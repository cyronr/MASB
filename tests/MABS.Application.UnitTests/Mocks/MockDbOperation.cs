using System.Data;

namespace MABS.Application.UnitTests.Mocks;

public static class MockDbOperation
{
    public static Mock<IDbOperation> GetDbOperation(bool withActivteTransaction = false)
    {
        var mockDbOperation = new Mock<IDbOperation>();

        mockDbOperation.Setup(r => r.Save()).Verifiable();
        mockDbOperation.Setup(r => r.BeginTransaction()).Returns(MockInternalDbTransaction.GetInternalDbTransaction().Object);
        mockDbOperation.Setup(r => r.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(MockInternalDbTransaction.GetInternalDbTransaction().Object);
        mockDbOperation.Setup(r => r.IsActiveTransaction()).Returns(withActivteTransaction);

        return mockDbOperation;
    }
}
