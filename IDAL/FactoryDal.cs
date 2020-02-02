namespace IDAL
{
    /// <summary>
    /// Get the Singelton Instance 
    /// </summary>
    public static class FactoryDal
    {
        private static DAL_XML_imp _instance;

        /// <summary>
        /// Get Instance of DAL
        /// </summary>
        public static DAL_XML_imp Instance => _instance ?? (_instance = new DAL_XML_imp());

    }
}