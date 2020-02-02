namespace BL
{
    /// <summary>
    /// Get the Singelton Instance 
    /// </summary>
    public static class FactoryBl
    {
        private static BlImp _instance;

        /// <summary>
        /// Get the instance of Bl_Imp 
        /// </summary>
        public static BlImp GetInstance => _instance ?? (_instance = new BlImp());
    }
}