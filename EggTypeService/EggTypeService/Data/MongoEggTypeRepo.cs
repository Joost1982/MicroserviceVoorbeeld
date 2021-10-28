using EggTypeService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Data
{

    public class MongoEggTypeRepo : IEggTypeRepo
    {
        private const string databaseName = "EggTypesDb";
        private const string collectionName = "EggTypes";
        private readonly IMongoCollection<EggType> itemsCollection;
        private readonly FilterDefinitionBuilder<EggType> filterBuilder = Builders<EggType>.Filter;
        private int id = 0;


        public MongoEggTypeRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<EggType>(collectionName);
            
            //set id (probeersel omdat we in deze db geen autoincrement hebben)
            //hele slechte implementatie, omdat die eerst de hele db door moet!
            IEnumerable<EggType> lijstCommands = GetAllEggTypes();
            foreach (EggType e in lijstCommands)
            {
                if (e.Id > id) id = e.Id;
            }
        }

        public void CreateEggType(EggType eggType)
        {
            id += 1;
            eggType.Id = id;
            itemsCollection.InsertOne(eggType);
        }


        public IEnumerable<EggType> GetAllEggTypes()
        {
            return  itemsCollection.Find(new BsonDocument()).ToList(); ;
        }

        public EggType GetEggTypeById(int id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public bool SaveChanges()
        {
            //return (_context.SaveChanges() >= 0);
            return true;
        }

    }

}
