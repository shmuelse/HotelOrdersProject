namespace BE
{
    public class enums
    {
        public enum Gender
        {
            Male,
            Female
        }
        public enum Type
        {
            Hotels,
            Apartments,
            Resorts,
            Villas,
            Cabins,
            Cottages,
            GlampingTent,
            VacationHomes,
            GuestHouses,
            Motels,
            BBS //B&Bs -> in hebrew = צימר
        }
        public enum Style
        {
            Boutique,
            Business,
            Family,
            Historic,
            Luxury,
            Romantic,
            Trendy
        }
        public enum Area
        {
            Jerusalem = 1,
            Northern = 2,
            Haifa = 3,
            Central = 4,
            TelAviv = 5,
            Southern = 6,
            JudeaAndSamaria = 7
        }
        public enum Districts
        {
            //Jerusalem
            Jerusalem = 1,

            //Northern
            Tzfat,
            Kinneret,
            Yizreel,
            Akko,
            Golan,

            //Haifa
            Haifa,
            Hadera,

            //Centeral
            Sharon,
            PetahTikva,
            Ramla,
            Rehovot,

            //TelAviv
            TelAviv,

            //Southern
            Ashkelon,
            BeerSheva,

            //JudeaAndSamaria
            JudeaAndSamaria
        }
        public enum OrderStatus
        {
            Pending,
            Completed,
            Cancelled,
            EmailSent,
            TimeOutCancellation,
        }
        public enum UnitUpdate
        {
            Price,
            OrderOffer,
            OrderCompleted,
            ElementsUpdate,
        }
    }
}