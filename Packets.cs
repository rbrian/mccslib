using System;
using System.IO;
using System.Net;
using System.Text;

namespace mccslib
{
	public static class StreamExtensions
	{
		#region Stream helpers
        public static int ReadInt(this Stream s)
        {
            return IPAddress.HostToNetworkOrder((int)Read(s, 4));
        }

        public static short ReadShort(this Stream s)
        {
            return IPAddress.HostToNetworkOrder((short)Read(s, 2));
        }

        public static long ReadLong(this Stream s)
        {
            return IPAddress.HostToNetworkOrder((long)Read(s, 8));
        }

        public static double ReadDouble(this Stream s)
        {
            return new BinaryReader(s).ReadDouble();
        }

        public static float ReadFloat(this Stream s)
        {
            return new BinaryReader(s).ReadSingle();
        }

        public static Boolean ReadBoolean(this Stream s)
        {
            return new BinaryReader(s).ReadBoolean();
        }
        
        public static byte[] ReadBytes(this Stream s, int count)
        {
            return new BinaryReader(s).ReadBytes(count);
        }

        public static String ReadString8(this Stream s)
        {
            short len = ReadShort(s);
            byte[] buf = new byte[len];
			s.Read(buf, 0, len);
            return Encoding.UTF8.GetString(buf);
        }

        public static void WriteString8(this Stream s, String msg)
		{
			WriteShort(s, (short)msg.Length);
		    byte[] buf = Encoding.UTF8.GetBytes(msg);
            s.Write(buf, 0, buf.Length);
        }

        public static String ReadString16(this Stream s)
        {
            short len = ReadShort(s);
            byte[] buf = new byte[len*2];
			s.Read(buf, 0, len*2);
            return Encoding.BigEndianUnicode.GetString(buf);
        }

        public static void WriteString16(this Stream s, String msg)
		{
			WriteShort(s, (short)msg.Length);
		    byte[] buf = Encoding.BigEndianUnicode.GetBytes(msg);
            s.Write(buf, 0, buf.Length);
        }

		public static void WriteInt(this Stream s, int i)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
            s.Write(a, 0, a.Length);
        }

        public static void WriteLong(this Stream s, long i)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
            s.Write(a, 0, a.Length);
        }

        public static void WriteShort(this Stream s, short i)
        {
            byte[] a = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
            s.Write(a, 0, a.Length);
        }

        public static void WriteDouble(this Stream s, double d)
        {
            new BinaryWriter(s).Write(d);
        }

        public static void WriteFloat(this Stream s, float f)
        {
            new BinaryWriter(s).Write(f);
        }

        public static void WriteBoolean(this Stream s, Boolean b)
        {
            new BinaryWriter(s).Write(b);
        }

        public static void WriteBytes(this Stream s, byte[] b)
        {
            new BinaryWriter(s).Write(b);
        }

        public static Object Read(this Stream s, int num)
        {
            byte[] b = new byte[num];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)s.ReadByte();
            }
            switch (num)
            {
                case 4:
                    return BitConverter.ToInt32(b, 0);
                case 8:
                    return BitConverter.ToInt64(b, 0);
                case 2:
                    return BitConverter.ToInt16(b, 0);
                default:
                    return 0;
            }
        }
		#endregion
		
		public static FaceOffset ReadFace(this Stream s)
		{
			return (FaceOffset)s.ReadByte();
		}
	}
	
	public abstract class PacketBase
	{
		public const int ProtocolVersion = 11;
		
		public PacketBase ()
		{
		}
		
		public abstract PacketType Type { get; }
		
		public virtual void ReadFromStream(Stream aStream)
		{
		}
		
		public virtual void WriteToStream(Stream aStream)
		{
			aStream.WriteByte((byte)Type);
		}
		
	}
	
	public class KeepAlive : PacketBase
	{
		public override PacketType Type { get { return PacketType.KeepAlive; }}
	}
	
	public class LoginRequest : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.LoginRequest;
			}
		}

		public int EntityID {
			get;
			set;
		}
		
		public string UserName {
			get;
			set;
		}
		
		public long MapSeed {
			get;
			set;
		}
		
		public byte Dimension {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt();
			UserName = aStream.ReadString16();
			MapSeed = aStream.ReadLong();
			Dimension = (byte)aStream.ReadByte();	
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream); // write ID
			aStream.WriteInt(EntityID);
			aStream.WriteString16(UserName);
			aStream.WriteLong(0);
			aStream.WriteByte(0);
		}
	}
	
	public class Handshake : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.Handshake;
			}
		}
		
		public string UserName {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			UserName = aStream.ReadString16();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteString16(UserName);
		}
	}
	
	public class ChatMsg : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.ChatMessage;
			}
		}
		
		public string Message {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			Message = aStream.ReadString16();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteString16(Message);
		}
	}
	
	public class TimeUpdate : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.TimeUpdate;
			}
		}
		
		public long Time {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			Time = aStream.ReadLong();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteLong(Time);
		}
	}
	
	public class EntityEquipment : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.EntityEquipment;
			}
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public short Slot {
			get;
			set;
		}
		
		public short ItemID {
			get;
			set;
		}
		
		public short Damage {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt();
			Slot = aStream.ReadShort();
			ItemID = aStream.ReadShort();
			Damage = aStream.ReadShort();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(EntityID);
			aStream.WriteShort(Slot);
			aStream.WriteShort(ItemID);
			aStream.WriteShort(Damage);
		}
	}
	
	public class SpawnPosition : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.SpawnPosition;
			}
		}
		
		public int X {
			get;
			set;
		}
		public int Y {
			get;
			set;
		}
		public int Z {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadInt();
			Y = aStream.ReadInt();
			Z = aStream.ReadInt();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(X);
			aStream.WriteInt(Y);
			aStream.WriteInt(Z);
		}
	}
	
	public class UseEntity : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.UseEntity;
			}
		}
		
		public int UserID {
			get;
			set;
		}
		
		public int TargetID {
			get;
			set;
		}
		
		public bool LeftClick {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			UserID = aStream.ReadInt();
			TargetID = aStream.ReadInt();
			LeftClick = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(UserID);
			aStream.WriteInt(TargetID);
			aStream.WriteBoolean(LeftClick);
		}
	}
	
	public class UpdateHealth : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.UpdateHealth;
			}
		}
		
		public short Health {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			Health = aStream.ReadShort();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteShort(Health);
		}
	}
	
	public class Respawn : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.Respawn;
			}
		}
	}
	
	public class Player : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.Player;
			}
		}
		
		public bool OnGround {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			OnGround = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteBoolean(OnGround);
		}
	}
	
	public class PlayerPosition : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.PlayerPosition;
			}
		}
		
		public double X {
			get;
			set;
		}
		
		public double Y {
			get;
			set;
		}
		
		public double Z {
			get;
			set;
		}
		
		public double Stance {
			get;
			set;
		}
		
		public bool OnGround {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble();
			Y = aStream.ReadDouble();
			Stance = aStream.ReadDouble();
			Z = aStream.ReadDouble();
			OnGround = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteDouble(X);
			aStream.WriteDouble(Y);
			aStream.WriteDouble(Stance);
			aStream.WriteDouble(Z);
			aStream.WriteBoolean(OnGround);
		}
	}	
	
	public class PlayerLook : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.PlayerLook;
			}
		}

		public float Yaw {
			get;
			set;
		}
		
		public float Pitch {
			get;
			set;
		}
		
		public bool OnGround {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			Yaw = aStream.ReadFloat();
			Pitch = aStream.ReadFloat();
			OnGround = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteFloat(Yaw);
			aStream.WriteFloat(Pitch);
			aStream.WriteBoolean(OnGround);
		}
	}
	
	public class PlayerPositionLookBase : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.PlayerPositionLook;
			}
		}
		
		public double X {
			get;
			set;
		}
		
		public double Y {
			get;
			set;
		}
		
		public double Z {
			get;
			set;
		}
		
		public double Stance {
			get;
			set;
		}
		
		public float Yaw {
			get;
			set;
		}
		
		public float Pitch {
			get;
			set;
		}
		public bool OnGround {
			get;
			set;
		}
	}
	
	public class PlayerPositionLookC2S : PlayerPositionLookBase
	{
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble();
			Y = aStream.ReadDouble();
			Stance = aStream.ReadDouble();
			Z = aStream.ReadDouble();
			Yaw = aStream.ReadFloat();
			Pitch = aStream.ReadFloat();
			OnGround = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			
			aStream.WriteDouble(X);
			aStream.WriteDouble(Y);
			aStream.WriteDouble(Stance);
			aStream.WriteDouble(Z);
			aStream.WriteFloat(Yaw);
			aStream.WriteFloat(Pitch);
			aStream.WriteBoolean(OnGround);
		}		
	}
	
	public class PlayerPositionLookS2C : PlayerPositionLookBase
	{
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble();
			Stance = aStream.ReadDouble();
			Y = aStream.ReadDouble();
			Z = aStream.ReadDouble();
			Yaw = aStream.ReadFloat();
			Pitch = aStream.ReadFloat();
			OnGround = aStream.ReadBoolean();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			
			aStream.WriteDouble(X);
			aStream.WriteDouble(Stance);
			aStream.WriteDouble(Y);
			aStream.WriteDouble(Z);
			aStream.WriteFloat(Yaw);
			aStream.WriteFloat(Pitch);
			aStream.WriteBoolean(OnGround);
		}		
	}
	
	public enum DigStatus : byte
	{
		Started = 0, Finished = 2, Drop = 4
	}
	
	public enum FaceOffset : byte
	{
		MinusY = 0, PlusY = 1, MinusZ = 2, PlusZ = 3, MinusX = 4, PlusX = 5
	}
	
	public class PlayerDigging : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.PlayerDigging;
			}
		}
		
		public DigStatus Status {
			get;
			set;
		}
		
		public int X {
			get;
			set;
		}
		
		public byte Y {
			get;
			set;
		}
		
		public int Z {
			get;
			set;
		}
		
		public FaceOffset Face {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			Status = (DigStatus)aStream.ReadByte();
			X = aStream.ReadInt();
			Y = (byte)aStream.ReadByte();
			Z = aStream.ReadInt();
			Face = aStream.ReadFace();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteByte((byte)Status);
			aStream.WriteInt(X);
			aStream.WriteByte(Y);
			aStream.WriteInt(Z);
			aStream.WriteByte((byte)Face);			
		}
	}
	
	public class PlayerBlockPlacement : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.PlayerBlockPlacement;
			}
		}
		
		public int X {
			get;
			set;
		}
		
		public byte Y {
			get;
			set;
		}
		
		public int Z {
			get;
			set;
		}

		public FaceOffset Direction {
			get;
			set;
		}
		
		public short ItemID {
			get;
			set;
		}
		
		public byte Amount {
			get;
			set;
		}
		
		public short Damage {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadInt();
			Y = (byte)aStream.ReadByte();
			Z = aStream.ReadInt();
			Direction = aStream.ReadFace();
			ItemID = aStream.ReadShort();
			if (ItemID >= 0)
			{
				Amount = (byte)aStream.ReadByte();
				Damage = aStream.ReadShort();
			}
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(X);
			aStream.WriteByte(Y);
			aStream.WriteInt(Z);
			aStream.WriteByte((byte)Direction);
			aStream.WriteShort(ItemID);
			if (ItemID >= 0)
			{
				aStream.WriteByte(Amount);
				aStream.WriteShort(Damage);
			}
		}
	}
	
	public class HoldingChange : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.HoldingChange;
			}
		}
		
		public short SlotID {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			SlotID = aStream.ReadShort();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteShort(SlotID);
		}
	}
	
	public class UseBed : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.UseBed;
			}
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public byte InBed {
			get;
			set;
		}
		
		public int X {
			get;
			set;
		}
		
		public byte Y {
			get;
			set;
		}
		
		public int Z {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt();
			InBed = (byte)aStream.ReadByte();
			X = aStream.ReadInt();
			Y = (byte)aStream.ReadByte();
			Z = aStream.ReadInt();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(EntityID);
			aStream.WriteByte(InBed);
			aStream.WriteInt(X);
			aStream.WriteByte(Y);
			aStream.WriteInt(Z);
		}
	}
	
	public class Animation : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.Animation;
			}
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public byte Animate {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt();
			Animate = (byte)aStream.ReadByte();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(EntityID);
			aStream.WriteByte(Animate);
		}
	}
	
	public class EntityAction : PacketBase
	{
		public override PacketType Type {
			get {
				return PacketType.EntityAction;
			}
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public byte Action {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt();
			Action = (byte)aStream.ReadByte();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(EntityID);
			aStream.WriteByte(Action);
		}
	}
	/*
      
        
        NamedEntitySpawn = 0x14,
        PickupSpawn = 0x15,
        CollectItem = 0x16,
        AddObjectVehicle = 0x17,
        MobSpawn = 0x18,
        EntityPainting = 0x19,
        EntityVelocity = 0x1C,
        DestroyEntity = 0x1D,
        Entity = 0x1E,
        EntityRelativeMove = 0x1F,
        EntityLook = 0x20,
        EntityLookRelativeMove = 0x21,
        EntityTeleport = 0x22,
        EntityStatus = 0x26,
        AttachEntity = 0x27,
        EntityMetadata = 0x28,
        PreChunk = 0x32,
        MapChunk = 0x33,
        MultiBlockChange = 0x34,
        BlockChange = 0x35,
        PlayNoteBlock = 0x36,
        Explosion = 0x3C,
        OpenWindow = 0x64,
        CloseWindow = 0x65,
        WindowClick = 0x66,
        SetSlot = 0x67,
        WindowItems = 0x68,
        UpdateProgressBar = 0x69,
        Transaction = 0x6A,
        UpdateSign = 0x82,
        DisconnectKick = 0xFF*/
}

