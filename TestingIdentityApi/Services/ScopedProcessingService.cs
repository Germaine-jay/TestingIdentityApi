namespace TestingIdentityApi.Services
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        public List<work> WorkList { get; set; }= new List<work>();
        public async Task DoWork(CancellationToken stoppingToken)
        {

            var worklist = new work
            {
                Id = 1,
                message = "Test",
            };

            var worklist2 = new work
            {
                Id = 2,
                message = "Test2",
            };

            var worklist3 = new work
            {
                Id = 3,
                message = "Test3",

            };

            var worklist4 = new work
            {
                Id = 4,
                message = "Test4",
            };
            WorkList.Add(worklist);
            WorkList.Add(worklist2);
            WorkList.Add(worklist3);
            WorkList.Add(worklist4);

            for(int i=0; i<WorkList.Count(); i++)
            {              
                 Console.WriteLine($"THIS APPLICATION IS WORKING WELL!!!! on {WorkList[i].message}");             
            }
        }

        public async Task SeedTask()
        {
           

        }
    }

    public class work
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }= DateTime.Now;
        public string message { get; set; }
    }
}
