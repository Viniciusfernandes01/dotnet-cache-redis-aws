using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using ServiceStack.Redis;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RedisLambdaTest
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            try
            {
                using (var redis = new RedisClient("redis-url", 6379))
                {
                    var redisUsers = redis.As<Person>();
                    LambdaLogger.Log("redis client created");

                    var user = new Person { Id = redisUsers.GetNextSequence(), Name = input };
                    redisUsers.Store(user);
                    LambdaLogger.Log("User added");

                    var allUsers = redisUsers.GetAll();
                    LambdaLogger.Log("Retrieved users");
                    return allUsers.Count + " " + allUsers[allUsers.Count - 1];
                }
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.StackTrace);
                return "Something went wrong";
            }
        }
    }
}
