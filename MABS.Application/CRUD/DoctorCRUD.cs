﻿using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.CRUD.Creators.DoctorCreator;
using MABS.Application.CRUD.Deleters.DoctorDeleter;
using MABS.Application.CRUD.Readers.DoctorReader;
using MABS.Application.CRUD.Updaters.DoctorUpdater;
using MABS.Application.DataAccess.Common;
using MABS.Application.Services;
using Microsoft.Extensions.Logging;

namespace MABS.Application.CRUD
{
    public interface IDoctorCRUD
    {
        public IDoctorCreator Creator { get; }
        public IDoctorReader Reader { get; }
        public IDoctorUpdater Updater { get; }
        public IDoctorDeleter Deleter { get; }
    }

    public class DoctorCRUD : IDoctorCRUD
    {
        public IDoctorCreator Creator { get; }
        public IDoctorReader Reader { get; }
        public IDoctorUpdater Updater { get; }
        public IDoctorDeleter Deleter { get; }

        public DoctorCRUD(
            IDoctorCreator creator,
            IDoctorReader reader,
            IDoctorUpdater updater,
            IDoctorDeleter deleter
            )
        {
            this.Creator = creator;
            this.Reader = reader;
            this.Updater = updater;
            this.Deleter = deleter;
        }
    }
}
