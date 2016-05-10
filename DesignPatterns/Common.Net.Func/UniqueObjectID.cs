/***************************************************************************** 
*        filename :UniqueObjectID 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   UniqueObjectID 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             UniqueObjectID 
*        创建系统时间:       2016/2/3 10:15:01 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Common.Net.Func
{
    /// <summary>
    /// 生成唯一ID，由MongoDB.Bson.BsonObjectId提取而来
    /// </summary>
    [Serializable]
    public class UniqueObjectID
    {
        // private static fields
        protected static UniqueObjectID __emptyInstance = default(UniqueObjectID);
        protected static int __staticMachine;
        protected static short __staticPid;
        protected static int __staticIncrement; // high byte will be masked out when generating new ObjectId

        // private fields
        // we're using 14 bytes instead of 12 to hold the ObjectId in memory but unlike a byte[] there is no additional object on the heap
        // the extra two bytes are not visible to anyone outside of this class and they buy us considerable simplification
        // an additional advantage of this representation is that it will serialize to JSON without any 64 bit overflow problems
        protected int _timestamp;
        protected int _machine;
        protected short _pid;
        protected int _increment;

        // static constructor
        static UniqueObjectID()
        {
            __staticMachine = (GetMachineHash() + AppDomain.CurrentDomain.Id) & 0x00ffffff; // add AppDomain Id to ensure uniqueness across AppDomains
            __staticIncrement = (new Random()).Next();

            try
            {
                __staticPid = (short)GetCurrentProcessId(); // use low order two bytes only
            }
            catch (SecurityException)
            {
                __staticPid = 0;
            }
        }

        // constructors
        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public UniqueObjectID(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            Unpack(bytes, out _timestamp, out _machine, out _pid, out _increment);
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="index">The index into the byte array where the ObjectId starts.</param>
        internal UniqueObjectID(byte[] bytes, int index)
        {
            _timestamp = (bytes[index] << 24) | (bytes[index + 1] << 16) | (bytes[index + 2] << 8) | bytes[index + 3];
            _machine = (bytes[index + 4] << 16) | (bytes[index + 5] << 8) | bytes[index + 6];
            _pid = (short)((bytes[index + 7] << 8) | bytes[index + 8]);
            _increment = (bytes[index + 9] << 16) | (bytes[index + 10] << 8) | bytes[index + 11];
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="timestamp">The timestamp (expressed as a DateTime).</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        public UniqueObjectID(DateTime timestamp, int machine, short pid, int increment)
            : this(GetTimestampFromDateTime(timestamp), machine, pid, increment)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        public UniqueObjectID(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            _timestamp = timestamp;
            _machine = machine;
            _pid = pid;
            _increment = increment;
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UniqueObjectID(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Unpack(BsonUtils.ParseHexString(value), out _timestamp, out _machine, out _pid, out _increment);
        }

        // public static properties
        /// <summary>
        /// Gets an instance of ObjectId where the value is empty.
        /// </summary>
        public static UniqueObjectID Empty
        {
            get { return __emptyInstance; }
        }

        // public properties
        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public int Timestamp
        {
            get { return _timestamp; }
        }

        /// <summary>
        /// Gets the machine.
        /// </summary>
        public int Machine
        {
            get { return _machine; }
        }

        /// <summary>
        /// Gets the PID.
        /// </summary>
        public short Pid
        {
            get { return _pid; }
        }

        /// <summary>
        /// Gets the increment.
        /// </summary>
        public int Increment
        {
            get { return _increment; }
        }

        /// <summary>
        /// Gets the creation time (derived from the timestamp).
        /// </summary>
        public DateTime CreationTime
        {
            get { return BsonConstants.UnixEpoch.AddSeconds(_timestamp); }
        }



        // public static methods
        /// <summary>
        /// Generates a new ObjectId with a unique value.
        /// </summary>
        /// <returns>An ObjectId.</returns>
        public static UniqueObjectID GenerateNewId()
        {
            return GenerateNewId(GetTimestampFromDateTime(DateTime.UtcNow));
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the timestamp component based on a given DateTime).
        /// </summary>
        /// <param name="timestamp">The timestamp component (expressed as a DateTime).</param>
        /// <returns>An ObjectId.</returns>
        public static UniqueObjectID GenerateNewId(DateTime timestamp)
        {
            return GenerateNewId(GetTimestampFromDateTime(timestamp));
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the given timestamp).
        /// </summary>
        /// <param name="timestamp">The timestamp component.</param>
        /// <returns>An ObjectId.</returns>
        public static UniqueObjectID GenerateNewId(int timestamp)
        {
            int increment = Interlocked.Increment(ref __staticIncrement) & 0x00ffffff; // only use low order 3 bytes
            return new UniqueObjectID(timestamp, __staticMachine, __staticPid, increment);
        }

        /// <summary>
        /// Packs the components of an ObjectId into a byte array.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        /// <returns>A byte array.</returns>
        public static byte[] Pack(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            byte[] bytes = new byte[12];
            bytes[0] = (byte)(timestamp >> 24);
            bytes[1] = (byte)(timestamp >> 16);
            bytes[2] = (byte)(timestamp >> 8);
            bytes[3] = (byte)(timestamp);
            bytes[4] = (byte)(machine >> 16);
            bytes[5] = (byte)(machine >> 8);
            bytes[6] = (byte)(machine);
            bytes[7] = (byte)(pid >> 8);
            bytes[8] = (byte)(pid);
            bytes[9] = (byte)(increment >> 16);
            bytes[10] = (byte)(increment >> 8);
            bytes[11] = (byte)(increment);
            return bytes;
        }

        /// <summary>
        /// Parses a string and creates a new ObjectId.
        /// </summary>
        /// <param name="s">The string value.</param>
        /// <returns>A ObjectId.</returns>
        public static UniqueObjectID Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            UniqueObjectID objectId;
            if (TryParse(s, out objectId))
            {
                return objectId;
            }
            else
            {
                var message = string.Format("'{0}' is not a valid 24 digit hex string.", s);
                throw new FormatException(message);
            }
        }

        /// <summary>
        /// Tries to parse a string and create a new ObjectId.
        /// </summary>
        /// <param name="s">The string value.</param>
        /// <param name="objectId">The new ObjectId.</param>
        /// <returns>True if the string was parsed successfully.</returns>
        public static bool TryParse(string s, out UniqueObjectID objectId)
        {
            // don't throw ArgumentNullException if s is null
            if (s != null && s.Length == 24)
            {
                byte[] bytes;
                if (BsonUtils.TryParseHexString(s, out bytes))
                {
                    objectId = new UniqueObjectID(bytes);
                    return true;
                }
            }

            objectId = default(UniqueObjectID);
            return false;
        }

        /// <summary>
        /// Unpacks a byte array into the components of an ObjectId.
        /// </summary>
        /// <param name="bytes">A byte array.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        public static void Unpack(byte[] bytes, out int timestamp, out int machine, out short pid, out int increment)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (bytes.Length != 12)
            {
                throw new ArgumentOutOfRangeException("bytes", "Byte array must be 12 bytes long.");
            }
            timestamp = (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3];
            machine = (bytes[4] << 16) + (bytes[5] << 8) + bytes[6];
            pid = (short)((bytes[7] << 8) + bytes[8]);
            increment = (bytes[9] << 16) + (bytes[10] << 8) + bytes[11];
        }

        // private static methods
        /// <summary>
        /// Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
        /// for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
        /// before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        protected static int GetMachineHash()
        {
            var hostName = Environment.MachineName; // use instead of Dns.HostName so it will work offline
            return 0x00ffffff & hostName.GetHashCode(); // use first 3 bytes of hash
        }

        public static int GetTimestampFromDateTime(DateTime timestamp)
        {
            var secondsSinceEpoch = (long)Math.Floor((BsonUtils.ToUniversalTime(timestamp) - BsonConstants.UnixEpoch).TotalSeconds);
            if (secondsSinceEpoch < int.MinValue || secondsSinceEpoch > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timestamp");
            }
            return (int)secondsSinceEpoch;
        }





        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = 37 * hash + _timestamp.GetHashCode();
            hash = 37 * hash + _machine.GetHashCode();
            hash = 37 * hash + _pid.GetHashCode();
            hash = 37 * hash + _increment.GetHashCode();
            return hash;
        }


        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns>A string representation of the value.</returns>
        public override string ToString()
        {
            return BsonUtils.ToHexString(Pack(_timestamp, _machine, _pid, _increment));
        }



    }

    /// <summary>
    /// A static class containing BSON constants.
    /// </summary>
    public static class BsonConstants
    {
        // private static fields
        private static readonly long __dateTimeMaxValueMillisecondsSinceEpoch;
        private static readonly long __dateTimeMinValueMillisecondsSinceEpoch;
        private static readonly DateTime __unixEpoch;

        // static constructor
        static BsonConstants()
        {
            // unixEpoch has to be initialized first
            __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            __dateTimeMaxValueMillisecondsSinceEpoch = (DateTime.MaxValue - __unixEpoch).Ticks / 10000;
            __dateTimeMinValueMillisecondsSinceEpoch = (DateTime.MinValue - __unixEpoch).Ticks / 10000;
        }

        // public static properties
        /// <summary>
        /// Gets the number of milliseconds since the Unix epoch for DateTime.MaxValue.
        /// </summary>
        public static long DateTimeMaxValueMillisecondsSinceEpoch
        {
            get { return __dateTimeMaxValueMillisecondsSinceEpoch; }
        }

        /// <summary>
        /// Gets the number of milliseconds since the Unix epoch for DateTime.MinValue.
        /// </summary>
        public static long DateTimeMinValueMillisecondsSinceEpoch
        {
            get { return __dateTimeMinValueMillisecondsSinceEpoch; }
        }

        /// <summary>
        /// Gets the Unix Epoch for BSON DateTimes (1970-01-01).
        /// </summary>
        public static DateTime UnixEpoch { get { return __unixEpoch; } }
    }

    /// <summary>
    /// A static class containing BSON utility methods.
    /// </summary>
    public static class BsonUtils
    {

        // public static methods
        /// <summary>
        /// Gets a friendly class name suitable for use in error messages.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A friendly class name.</returns>
        public static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("{0}<", Regex.Replace(type.Name, @"\`\d+$", ""));
            foreach (var typeParameter in type.GetGenericArguments())
            {
                sb.AppendFormat("{0}, ", GetFriendlyTypeName(typeParameter));
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(">");
            return sb.ToString();
        }

        /// <summary>
        /// Parses a hex string into its equivalent byte array.
        /// </summary>
        /// <param name="s">The hex string to parse.</param>
        /// <returns>The byte equivalent of the hex string.</returns>
        public static byte[] ParseHexString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            byte[] bytes;
            if ((s.Length & 1) != 0)
            {
                s = "0" + s; // make length of s even
            }
            bytes = new byte[s.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = s.Substring(2 * i, 2);
                try
                {
                    byte b = Convert.ToByte(hex, 16);
                    bytes[i] = b;
                }
                catch (FormatException e)
                {
                    throw new FormatException(
                        string.Format("Invalid hex string {0}. Problem with substring {1} starting at position {2}",
                        s,
                        hex,
                        2 * i),
                        e);
                }
            }

            return bytes;
        }

        /// <summary>
        /// Converts from number of milliseconds since Unix epoch to DateTime.
        /// </summary>
        /// <param name="millisecondsSinceEpoch">The number of milliseconds since Unix epoch.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime ToDateTimeFromMillisecondsSinceEpoch(long millisecondsSinceEpoch)
        {
            if (millisecondsSinceEpoch < BsonConstants.DateTimeMinValueMillisecondsSinceEpoch ||
                millisecondsSinceEpoch > BsonConstants.DateTimeMaxValueMillisecondsSinceEpoch)
            {
                var message = string.Format(
                    "The value {0} for the BsonDateTime MillisecondsSinceEpoch is outside the" +
                    "range that can be converted to a .NET DateTime.",
                    millisecondsSinceEpoch);
                throw new ArgumentOutOfRangeException("millisecondsSinceEpoch", message);
            }

            // MaxValue has to be handled specially to avoid rounding errors
            if (millisecondsSinceEpoch == BsonConstants.DateTimeMaxValueMillisecondsSinceEpoch)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return BsonConstants.UnixEpoch.AddTicks(millisecondsSinceEpoch * 10000);
            }
        }

        /// <summary>
        /// Converts a byte array to a hex string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>A hex string.</returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a DateTime to local time (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in local time.</returns>
        public static DateTime ToLocalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Local);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Local);
            }
            else
            {
                return dateTime.ToLocalTime();
            }
        }

        /// <summary>
        /// Converts a DateTime to number of milliseconds since Unix epoch.
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>Number of seconds since Unix epoch.</returns>
        public static long ToMillisecondsSinceEpoch(DateTime dateTime)
        {
            var utcDateTime = ToUniversalTime(dateTime);
            return (utcDateTime - BsonConstants.UnixEpoch).Ticks / 10000;
        }

        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

        /// <summary>
        /// Tries to parse a hex string to a byte array.
        /// </summary>
        /// <param name="s">The hex string.</param>
        /// <param name="bytes">A byte array.</param>
        /// <returns>True if the hex string was successfully parsed.</returns>
        public static bool TryParseHexString(string s, out byte[] bytes)
        {
            try
            {
                bytes = ParseHexString(s);
            }
            catch
            {
                bytes = null;
                return false;
            }

            return true;
        }
    }
}
