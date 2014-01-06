/*
 * Created by SharpDevelop.
 * User: BORG
 * Date: 2013-04-30
 * Time: 20:16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace BorgNetLib
{
	/// <summary>
	/// Description of CacheService.
	/// </summary>
	public static class CacheService
	{
		private static HttpRuntime _httpRuntime;

      public static Cache Cache
      {
         get
         {
            EnsureHttpRuntime();
            return HttpRuntime.Cache;
         }
      }
      public static void Add(String Key, String Value){
      	
      	Cache.Insert( 
            Key,
            Value, 
            null, 
            Cache.NoAbsoluteExpiration,
            TimeSpan.FromSeconds( 3 ) );
      }
       public static void Add(String Key, Object Value){
      	
      	Cache.Insert( 
            Key,
            Value, 
            null, 
            Cache.NoAbsoluteExpiration,
            TimeSpan.FromSeconds( 3 ) );
      }

       public static void Add(String Key, Object Value, int SecondsToLive){
      	
      	Cache.Insert( 
            Key,
            Value, 
            null, 
            Cache.NoAbsoluteExpiration,
            TimeSpan.FromSeconds( SecondsToLive ) );
      }
      
      public static object Get(String Key){
      	
         object value = Cache[ Key ];
	     return value;
      }
      
       public static String GetString(String Key){
      	
         String value = Cache[ Key ] as String;
	     return value;
      }     
      
      public static void Remove(String key){
      	Cache.Remove(key);
      }
      
       private static void EnsureHttpRuntime()
      {
         if( null == _httpRuntime )
         {
            try
            {
               Monitor.Enter( typeof( NetService ) );
               if( null == _httpRuntime )
               {
                  // Create an Http Content to give us access to the cache.
                  _httpRuntime = new HttpRuntime();
               }
            }
            finally
            {
               Monitor.Exit( typeof( NetService ) );
            }
         }
      }
		
	}
	
}
