namespace MABS.Application.UnitTests.Mocks;
public static class MockInternalDbTransaction
{
    public static Mock<IInternalDbTransaction> GetInternalDbTransaction()
    {
        var mockInternalDbTransaction = new Mock<IInternalDbTransaction>();

        mockInternalDbTransaction.Setup(r => r.Commit()).Verifiable();
        mockInternalDbTransaction.Setup(r => r.Rollback()).Verifiable();

        return mockInternalDbTransaction;
    }
}
