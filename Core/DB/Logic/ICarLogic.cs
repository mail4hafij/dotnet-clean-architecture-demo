namespace Core.DB.Logic
{
    /// <summary>
    /// SIMPLE EXAMPLE: Logic class that only depends on repositories
    /// Business logic for Car operations
    /// </summary>
    public interface ICarLogic
    {
        /// <summary>
        /// Validates and adds a car for a user
        /// Business rules:
        /// - User must exist
        /// - User cannot have duplicate car nameplates
        /// - Nameplate must be at least 3 characters
        /// </summary>
        void AddCarWithValidation(long userId, string nameplate);

        /// <summary>
        /// Transfers car ownership from one user to another
        /// Business rules:
        /// - Both users must exist
        /// - Car must belong to source user
        /// </summary>
        void TransferCarOwnership(long carId, long fromUserId, long toUserId);

        /// <summary>
        /// Gets the total number of cars for a user
        /// </summary>
        int GetUserCarCount(long userId);
    }
}
