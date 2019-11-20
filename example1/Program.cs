using System;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;

namespace example1
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = "Data Source=(local);Initial Catalog=DapperExample;Integrated Security=SSPI";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int result = 0;

                var eventName = connection.QueryFirst<string>("SELECT TOP 1 EventName FROM Event WHERE Id = 1");
                Console.WriteLine(eventName);

                // consultar un item con id=1 y mapearlo a Event
                var myEvent = connection.QueryFirst<Event>("SELECT * FROM Event WHERE Id = 1");
                Console.WriteLine(myEvent.Id + " : " + myEvent.EventName);

                // consulta con parametros
                int eventId = 1;
                var myEvent1 = connection.QueryFirst<EventDto>("SELECT Id, EventName FROM Event WHERE Id = @Id", new { Id = eventId });
                Console.WriteLine(myEvent1.Id + " : " + myEvent1.EventName);

                // consultar varias filas
                var allEvents = connection.Query<EventDto>("SELECT Id, EventName FROM Event");
                foreach (var myEvent2 in allEvents)
                {
                    Console.WriteLine(myEvent2.Id + " : " + myEvent2.EventName);
                }

                // actualizacion
                result = connection.Execute("UPDATE Event SET EventName = 'NewEventName' WHERE Id = 1");

                // actualizacion con parametros
                var eventName1 = "NewEventName";
                var eventId1 = 1;
                connection.Execute("UPDATE Event SET EventName = @EventName WHERE Id = @EventId", new { EventName = eventName1, EventId = eventId1 });

                // insertar un registro
                connection.Execute("INSERT INTO Event (EventLocationId, EventName, EventDate, DateCreated) VALUES(1, 'InsertedEvent', '2019-01-01', GETUTCDATE())");

                // eliminar un registro
                connection.Execute("DELETE FROM Event WHERE Id = 4");

                // insertar registros con Dapper.Contrib
                var newEvent = new Event
                {
                    EventLocationId = 1,
                    EventName = "Contrib Inserted Event",
                    EventDate = DateTime.Now.AddDays(1),
                    DateCreated = DateTime.UtcNow
                };
                var id = connection.Insert(newEvent);

                // obtener registro por dapper.contrib
                var eventId3 = 1;
                var myEvent3 = connection.Get<Event>(eventId3);


                // actualizacion
                myEvent3.EventName = "New Name";
                connection.Update(myEvent3);

                // eliminar usando dapper.contrib
                connection.Delete(new Event { Id = 5 });

                Console.ReadLine();
            }
            Console.WriteLine("Hello World!");
        }
    }
}
