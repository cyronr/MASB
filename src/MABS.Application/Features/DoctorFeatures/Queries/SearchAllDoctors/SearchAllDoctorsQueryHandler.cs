using MABS.Application.Common.Geolocation;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Elasticsearch;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.ModelsExtensions.DictionaryModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctors
{
    public class SearchAllDoctorsQueryHandler : IRequestHandler<SearchAllDoctorsQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<SearchAllDoctorsQueryHandler> _logger;
        private readonly IElasticsearchDoctorService _elasticsearch;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IGeolocator _geolocator;

        public SearchAllDoctorsQueryHandler(
            ILogger<SearchAllDoctorsQueryHandler> logger,
            IElasticsearchDoctorService elasticsearch,
            IFacilityRepository facilityRepository,
            IDictionaryRepository dictionaryRepository,
            IGeolocator geolocator)
        {
            _logger = logger;
            _elasticsearch = elasticsearch;
            _facilityRepository = facilityRepository;
            _dictionaryRepository = dictionaryRepository;
            _geolocator = geolocator;
        }

        public async Task<PagedList<DoctorDto>> Handle(SearchAllDoctorsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctors with paging parameters = {query.PagingParameters.ToString()}.");

            if (query.SearchText is null && query.SpecialtyId is null && query.CityId is null)
                return PagedList<DoctorDto>.ToPagedList(new List<DoctorDto>(), 0, 0); 

            var searchResult = await _elasticsearch.SearchAsync(query.SearchText, query.SpecialtyId);
            var doctors = searchResult.Select(d => d.ConvertToDoctorDto()).ToList();
            
            if (query.CityId is not null)
            {
                var city = await new City().GetByIdAsync(_dictionaryRepository, (int)query.CityId);
                doctors = await FilterDoctorsByCity(doctors, city);
            }

            return PagedList<DoctorDto>.ToPagedList(
                doctors,
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

        private async Task<List<DoctorDto>> FilterDoctorsByCity(List<DoctorDto> doctors, City city)
        {
            var filteredDoctors = new List<DoctorDto>();

            var cityCoordinates = new GeoCoordinates(city.Latitude, city.Longitude);
            
            foreach (var doctor in doctors)
            {
                foreach(var doctorFacility in doctor.Facilities)
                {
                    var facility = await new Facility().GetByUUIDAsync(_facilityRepository, doctorFacility.Id);
                    foreach(var address in facility.Addresses)
                    {
                        var addressCoordinates = new GeoCoordinates(address.Latitude, address.Longitude);
                        double distance = _geolocator.CalculateDistanceBetweenPoints(cityCoordinates, addressCoordinates);

                        if (distance < 10000)
                        {
                            filteredDoctors.Add(doctor);
                            continue;
                        }
                    }

                    if (filteredDoctors.FirstOrDefault(d => d.Id == doctor.Id) is not null)
                        continue;
                }
            }
            
            return filteredDoctors;
        }
    }
}
