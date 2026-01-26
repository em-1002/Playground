

using Playground.DataModels;

namespace Playground
{
    public class DatabaseBroker
    {
        private const string PATH_TO_DATABASE = @"\\devshares\Shares\BINARS\ExampleDataFile.db";
        private readonly SqlDataAccess access;

        public DatabaseBroker()
        {
            access = new(PATH_TO_DATABASE);
        }



        internal async Task<List<Vector>> GetVectorsAsync()
            => [.. await access.GetDataASYNC<Vector>("SELECT * FROM Vectors")];

        /// <summary>
        /// This task returns records based off a passed in VectorId
        /// </summary>
        /// <param name="vectorId"></param>
        /// <returns></returns>
        internal async Task<List<DataRecord>> GetDataRecordsAsync(int vectorId)
        {
            string sql = $"Select * from RValues Where VectBlockTimeId in (Select VectBlockTimeId From VectBlockTimes WHERE VectBlockId in (Select VectBlockId From VectBlocks WHERE VectorId == '{vectorId}'))";
            var RVals = await access.GetDataASYNC<RValue>(sql);

            sql = $"Select * from XValues Where VectBlockTimeId in (Select VectBlockTimeId From VectBlockTimes WHERE VectBlockId in (Select VectBlockId From VectBlocks WHERE VectorId == '{vectorId}'))";
            var XVals = await access.GetDataASYNC<XValue>(sql);

            List<DataRecord> records = [];
            if (RVals.Count == XVals.Count)
            {
                for (int i = 0; i < RVals.Count; i++)
                {
                    records.Add(new DataRecord(RVals[i], XVals[i]));
                }
            }

            return records;
        }
    }
}
