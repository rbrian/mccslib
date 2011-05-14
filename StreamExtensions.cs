using System;
using System.IO;
using System.Net;
using System.Text;

namespace mccslib
{
	public static class StreamExtensions
	{
		#region Stream helpers
		public static byte ReadByte2 (this Stream s)
		{
			return (byte)s.ReadByte ();
		}

		public static int ReadInt (this Stream s)
		{
			return IPAddress.HostToNetworkOrder ((int)Read (s, 4));
		}

		public static short ReadShort (this Stream s)
		{
			return IPAddress.HostToNetworkOrder ((short)Read (s, 2));
		}

		public static long ReadLong (this Stream s)
		{
			return IPAddress.HostToNetworkOrder ((long)Read (s, 8));
		}

		public static double ReadDouble (this Stream s)
		{
			return new BinaryReader (s).ReadDouble ();
		}

		public static float ReadFloat (this Stream s)
		{
			return new BinaryReader (s).ReadSingle ();
		}

		public static Boolean ReadBoolean (this Stream s)
		{
			return new BinaryReader (s).ReadBoolean ();
		}

		public static byte[] ReadBytes (this Stream s, int count)
		{
			return new BinaryReader (s).ReadBytes (count);
		}

		public static String ReadString8 (this Stream s)
		{
			short len = ReadShort (s);
			byte[] buf = new byte[len];
			s.Read (buf, 0, len);
			return Encoding.UTF8.GetString (buf);
		}

		public static void WriteString8 (this Stream s, String msg)
		{
			WriteShort (s, (short)msg.Length);
			byte[] buf = Encoding.UTF8.GetBytes (msg);
			s.Write (buf, 0, buf.Length);
		}

		public static String ReadString16 (this Stream s)
		{
			short len = ReadShort (s);
			byte[] buf = new byte[len * 2];
			s.Read (buf, 0, len * 2);
			return Encoding.BigEndianUnicode.GetString (buf);
		}

		public static void WriteString16 (this Stream s, String msg)
		{
			WriteShort (s, (short)msg.Length);
			byte[] buf = Encoding.BigEndianUnicode.GetBytes (msg);
			s.Write (buf, 0, buf.Length);
		}

		public static void WriteInt (this Stream s, int i)
		{
			byte[] a = BitConverter.GetBytes (IPAddress.HostToNetworkOrder (i));
			s.Write (a, 0, a.Length);
		}

		public static void WriteLong (this Stream s, long i)
		{
			byte[] a = BitConverter.GetBytes (IPAddress.HostToNetworkOrder (i));
			s.Write (a, 0, a.Length);
		}

		public static void WriteShort (this Stream s, short i)
		{
			byte[] a = BitConverter.GetBytes (IPAddress.HostToNetworkOrder (i));
			s.Write (a, 0, a.Length);
		}

		public static void WriteDouble (this Stream s, double d)
		{
			new BinaryWriter (s).Write (d);
		}

		public static void WriteFloat (this Stream s, float f)
		{
			new BinaryWriter (s).Write (f);
		}

		public static void WriteBoolean (this Stream s, Boolean b)
		{
			new BinaryWriter (s).Write (b);
		}

		public static void WriteBytes (this Stream s, byte[] b)
		{
			new BinaryWriter (s).Write (b);
		}

		public static Object Read (this Stream s, int num)
		{
			byte[] b = new byte[num];
			for (int i = 0; i < b.Length; i++) {
				b [i] = (byte)s.ReadByte ();
			}
			switch (num) {
			case 4:
				return BitConverter.ToInt32 (b, 0);
			case 8:
				return BitConverter.ToInt64 (b, 0);
			case 2:
				return BitConverter.ToInt16 (b, 0);
			default:
				return 0;
			}
		}

		#endregion

		public static FaceOffset ReadFace (this Stream s)
		{
			return (FaceOffset)s.ReadByte ();
		}
		
		public static MovingObject ReadObjType (this Stream s)
		{
			return (MovingObject)s.ReadByte ();
		}
		
		public static MobType ReadMobType(this Stream s)
		{
			return (MobType)s.ReadByte();
		}
	}
}

