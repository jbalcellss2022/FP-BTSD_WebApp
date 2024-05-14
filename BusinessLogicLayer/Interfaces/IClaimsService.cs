namespace BusinessLogicLayer.Interfaces
{
    public interface IClaimsService
    {
        /// <summary>
        /// Get the value of a claim from the user.
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        string GetClaimValue(string ClaimType);
    }
}
