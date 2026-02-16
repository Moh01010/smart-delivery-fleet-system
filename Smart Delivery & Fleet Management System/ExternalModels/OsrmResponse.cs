using System.Text.Json.Serialization;
namespace Smart_Delivery___Fleet_Management_System.ExternalModels
{
    public class OsrmResponse
    {
        public List<Route> routes { get; set; }

        public class Route
        {
            public double duration { get; set; }
            public double distance { get; set; }
        }
    }
}
