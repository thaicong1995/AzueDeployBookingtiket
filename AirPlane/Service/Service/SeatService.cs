//using Microsoft.Extensions.Caching.Memory;
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;

//namespace AirPlane.Service.Service
//{
//    public class SeatService
//    {
//        private readonly IDatabase _redisDatabase;
//        private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(3);

//        public SeatService(IDatabase redisDatabase)
//        {
//            _redisDatabase = redisDatabase;
//        }

//        public void ReserveSeat(string flightNumber, string seatNumber)
//        {
//            string key = GetSeatKey(flightNumber, seatNumber);
//            _redisDatabase.StringSet(key, "reserved", _expirationTime);
//        }

//        public bool IsSeatReserved(string flightNumber, string seatNumber)
//        {
//            string key = GetSeatKey(flightNumber, seatNumber);
//            return _redisDatabase.KeyExists(key);
//        }

//        private string GetSeatKey(string flightNumber, string seatNumber)
//        {
//            return $"flight:{flightNumber}:seat:{seatNumber}";
//        }
//    }
//}


///// su dung redis luu tru chuyen bay va string ghe da duoc chon trong 3 p. 
///// o phan select. Kiem tra redis co du lieu hay khong. Neu co thi tam thoi xu ly khoa ghe trong khi nguoi dung chon ma chua thanh toan.
///// neu thanh toan thanh cong thi cung clear cach voi so ghe do. 