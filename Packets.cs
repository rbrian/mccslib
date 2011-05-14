using System;
using System.IO;
using System.Net;
using System.Text;

namespace mccslib
{
	public abstract class PacketBase
	{
		public const int ProtocolVersion = 11;

		public PacketBase ()
		{
		}

		public abstract PacketType PacketType { get; }

		public virtual void ReadFromStream (Stream aStream)
		{
		}

		public virtual void WriteToStream (Stream aStream)
		{
			aStream.WriteByte ((byte)PacketType);
		}
		
	}

	public class KeepAlive : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.KeepAlive; }
		}
	}

	public class LoginRequest : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.LoginRequest; }
		}

		public int EntityID { get; set; }

		public string UserName { get; set; }

		public long MapSeed { get; set; }

		public byte Dimension { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			UserName = aStream.ReadString16 ();
			MapSeed = aStream.ReadLong ();
			Dimension = aStream.ReadByte2 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			// write ID
			aStream.WriteInt (EntityID);
			aStream.WriteString16 (UserName);
			aStream.WriteLong (0);
			aStream.WriteByte (0);
		}
	}

	public class Handshake : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Handshake; }
		}

		public string UserName { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			UserName = aStream.ReadString16 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteString16 (UserName);
		}
	}

	public class ChatMsg : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.ChatMessage; }
		}

		public string Message { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			Message = aStream.ReadString16 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteString16 (Message);
		}
	}

	public class TimeUpdate : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.TimeUpdate; }
		}

		public long Time { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			Time = aStream.ReadLong ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteLong (Time);
		}
	}

	public class EntityEquipment : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.EntityEquipment; }
		}

		public int EntityID { get; set; }

		public short Slot { get; set; }

		public short ItemID { get; set; }

		public short Damage { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Slot = aStream.ReadShort ();
			ItemID = aStream.ReadShort ();
			Damage = aStream.ReadShort ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteShort (Slot);
			aStream.WriteShort (ItemID);
			aStream.WriteShort (Damage);
		}
	}

	public class SpawnPosition : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.SpawnPosition; }
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);
		}
	}

	public class UseEntity : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.UseEntity; }
		}

		public int UserID { get; set; }

		public int TargetID { get; set; }

		public bool LeftClick { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			UserID = aStream.ReadInt ();
			TargetID = aStream.ReadInt ();
			LeftClick = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (UserID);
			aStream.WriteInt (TargetID);
			aStream.WriteBoolean (LeftClick);
		}
	}

	public class UpdateHealth : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.UpdateHealth; }
		}

		public short Health { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			Health = aStream.ReadShort ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteShort (Health);
		}
	}

	public class Respawn : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Respawn; }
		}
	}

	public class Player : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Player; }
		}

		public bool OnGround { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			OnGround = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteBoolean (OnGround);
		}
	}

	public class PlayerPosition : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayerPosition; }
		}

		public double X { get; set; }

		public double Y { get; set; }

		public double Z { get; set; }

		public double Stance { get; set; }

		public bool OnGround { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble ();
			Y = aStream.ReadDouble ();
			Stance = aStream.ReadDouble ();
			Z = aStream.ReadDouble ();
			OnGround = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteDouble (X);
			aStream.WriteDouble (Y);
			aStream.WriteDouble (Stance);
			aStream.WriteDouble (Z);
			aStream.WriteBoolean (OnGround);
		}
	}

	public class PlayerLook : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayerLook; }
		}

		public float Yaw { get; set; }

		public float Pitch { get; set; }

		public bool OnGround { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			Yaw = aStream.ReadFloat ();
			Pitch = aStream.ReadFloat ();
			OnGround = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteFloat (Yaw);
			aStream.WriteFloat (Pitch);
			aStream.WriteBoolean (OnGround);
		}
	}

	public class PlayerPositionLookBase : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayerPositionLook; }
		}

		public double X { get; set; }

		public double Y { get; set; }

		public double Z { get; set; }

		public double Stance { get; set; }

		public float Yaw { get; set; }

		public float Pitch { get; set; }

		public bool OnGround { get; set; }
	}

	public class PlayerPositionLookC2S : PlayerPositionLookBase
	{
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble ();
			Y = aStream.ReadDouble ();
			Stance = aStream.ReadDouble ();
			Z = aStream.ReadDouble ();
			Yaw = aStream.ReadFloat ();
			Pitch = aStream.ReadFloat ();
			OnGround = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			
			aStream.WriteDouble (X);
			aStream.WriteDouble (Y);
			aStream.WriteDouble (Stance);
			aStream.WriteDouble (Z);
			aStream.WriteFloat (Yaw);
			aStream.WriteFloat (Pitch);
			aStream.WriteBoolean (OnGround);
		}
	}

	public class PlayerPositionLookS2C : PlayerPositionLookBase
	{
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadDouble ();
			Stance = aStream.ReadDouble ();
			Y = aStream.ReadDouble ();
			Z = aStream.ReadDouble ();
			Yaw = aStream.ReadFloat ();
			Pitch = aStream.ReadFloat ();
			OnGround = aStream.ReadBoolean ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			
			aStream.WriteDouble (X);
			aStream.WriteDouble (Stance);
			aStream.WriteDouble (Y);
			aStream.WriteDouble (Z);
			aStream.WriteFloat (Yaw);
			aStream.WriteFloat (Pitch);
			aStream.WriteBoolean (OnGround);
		}
	}

	public class PlayerDigging : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayerDigging; }
		}

		public DigStatus Status { get; set; }

		public int X { get; set; }

		public byte Y { get; set; }

		public int Z { get; set; }

		public FaceOffset Face { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			Status = (DigStatus)aStream.ReadByte ();
			X = aStream.ReadInt ();
			Y = aStream.ReadByte2 ();
			Z = aStream.ReadInt ();
			Face = aStream.ReadFace ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteByte ((byte)Status);
			aStream.WriteInt (X);
			aStream.WriteByte (Y);
			aStream.WriteInt (Z);
			aStream.WriteByte ((byte)Face);
		}
	}

	public class PlayerBlockPlacement : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayerBlockPlacement; }
		}

		public int X { get; set; }

		public byte Y { get; set; }

		public int Z { get; set; }

		public FaceOffset Direction { get; set; }

		public short ItemID { get; set; }

		public byte Amount { get; set; }

		public short Damage { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadInt ();
			Y = aStream.ReadByte2 ();
			Z = aStream.ReadInt ();
			Direction = aStream.ReadFace ();
			ItemID = aStream.ReadShort ();
			if (ItemID >= 0) {
				Amount = aStream.ReadByte2 ();
				Damage = aStream.ReadShort ();
			}
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (X);
			aStream.WriteByte (Y);
			aStream.WriteInt (Z);
			aStream.WriteByte ((byte)Direction);
			aStream.WriteShort (ItemID);
			if (ItemID >= 0) {
				aStream.WriteByte (Amount);
				aStream.WriteShort (Damage);
			}
		}
	}

	public class HoldingChange : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.HoldingChange; }
		}

		public short SlotID { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			SlotID = aStream.ReadShort ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteShort (SlotID);
		}
	}

	public class UseBed : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.UseBed; }
		}

		public int EntityID { get; set; }

		public byte InBed { get; set; }

		public int X { get; set; }

		public byte Y { get; set; }

		public int Z { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			InBed = aStream.ReadByte2 ();
			X = aStream.ReadInt ();
			Y = aStream.ReadByte2 ();
			Z = aStream.ReadInt ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteByte (InBed);
			aStream.WriteInt (X);
			aStream.WriteByte (Y);
			aStream.WriteInt (Z);
		}
	}

	public class Animation : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Animation; }
		}

		public int EntityID { get; set; }

		public byte Animate { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Animate = aStream.ReadByte2 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteByte (Animate);
		}
	}

	public class EntityAction : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.EntityAction; }
		}

		public int EntityID { get; set; }

		public byte Action { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Action = aStream.ReadByte2 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteByte (Action);
		}
	}

	public class NamedEntitySpawn : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.NamedEntitySpawn; }
		}

		public int EntityID { get; set; }

		public string PlayerName { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }

		public byte Rotation { get; set; }

		public byte Pitch { get; set; }

		public short CurrentItem { get; set; }

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			PlayerName = aStream.ReadString16 ();
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Rotation = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
			CurrentItem = aStream.ReadShort ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteString16 (PlayerName);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);
			aStream.WriteByte (Rotation);
			aStream.WriteByte (Pitch);
			aStream.WriteShort (CurrentItem);
		}
	}

	public class PickupSpawn : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PickupSpawn; }
		}

		public int EntityID { get; set; }

		public short ItemID { get; set; }

		public byte Count { get; set; }

		public short DamageData {
			get;
			set;
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

		public byte Rotation {
			get;
			set;
		}

		public byte Pitch {
			get;
			set;
		}

		public byte Roll {
			get;
			set;
		}

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			ItemID = aStream.ReadShort ();
			Count = aStream.ReadByte2 ();
			DamageData = aStream.ReadShort ();
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Rotation = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
			Roll = aStream.ReadByte2 ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteShort (ItemID);
			aStream.WriteByte (Count);
			aStream.WriteShort (DamageData);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);
			aStream.WriteByte (Rotation);
			aStream.WriteByte (Pitch);
			aStream.WriteByte (Roll);
		}
	}

	public class CollectItem : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.CollectItem; }
		}
		
		public int CollectedEID {
			get;
			set;
		}
	
		public int CollectorEID {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			CollectedEID = aStream.ReadInt ();
			CollectorEID = aStream.ReadInt ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (CollectedEID);
			aStream.WriteInt (CollectorEID);
		}
	}

	public class AddObjectVehicle : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.AddObjectVehicle; }
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public MovingObject Type {
			get;
			set;
		}		
		
		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Type = aStream.ReadObjType ();
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteByte ((byte)Type);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);		
		}
	}

	public class MobSpawn : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.MobSpawn; }
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public MobType Type {
			get;
			set;
		}
		
		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }
	
		public byte Yaw { get; set; }
		
		public byte Pitch { get; set; }
		
		public MetadataDict Metadata {
			get;
			private set;
		}
		
		public MobSpawn ()
		{
			Metadata = new MetadataDict ();
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Type = aStream.ReadMobType ();
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Yaw = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
			Metadata = MetadataDict.ReadFromStream (aStream);
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteByte ((byte)Type);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);		
			aStream.WriteByte (Yaw);
			aStream.WriteByte (Pitch);
			Metadata.WriteToStream(aStream);
		}
	}

	public class EntityPainting : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.EntityPainting; }
		}
		
		public int EntityID {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }
		
		public int Direction { get; set; }
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			Title = aStream.ReadString16 ();
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Direction = aStream.ReadInt ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteString16 (Title);
			aStream.WriteInt (X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);		
			aStream.WriteInt (Direction);		
		}
	}
	
	public class Unknown1B : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Unknown1B; }
		}
		
		public float A { get; set; }
		public float B { get; set; }
		public bool C { get; set; }
		public bool D { get; set; }
		public float E { get; set; }

		public float F { get; set; }
		
		public override void ReadFromStream (Stream aStream)
		{
			A = aStream.ReadFloat ();
			B = aStream.ReadFloat ();
			C = aStream.ReadBoolean ();
			D = aStream.ReadBoolean ();
			E = aStream.ReadFloat ();
			F = aStream.ReadFloat ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteFloat(A);
			aStream.WriteFloat (B);
			aStream.WriteBoolean (C);
			aStream.WriteBoolean (D);
			aStream.WriteFloat (E);
			aStream.WriteFloat (F);
		}
	}
	
	public class EntityVelocity : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.EntityVelocity; }
		}
		
		public int EntityID {
			get;
			set;
		}
		
		public short VelX {
			get;
			set;
		}
		public short VelY {
			get;
			set;
		}
		public short VelZ {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
			VelX = aStream.ReadShort ();
			VelY = aStream.ReadShort ();
			VelZ = aStream.ReadShort ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
			aStream.WriteShort(VelX);
			aStream.WriteShort (VelY);
			aStream.WriteShort (VelZ);
		}
	}

	public class DestroyEntity : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.DestroyEntity; }
		}

		public int EntityID {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
		}
	}

	public class Entity : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Entity; }
		}

		public int EntityID {
			get;
			set;
		}

		public override void ReadFromStream (Stream aStream)
		{
			EntityID = aStream.ReadInt ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (EntityID);
		}
	}

	public class EntityRelativeMove : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityRelativeMove; }
		}
		
		public byte dX {
			get;
			set;
		}
		public byte dY {
			get;
			set;
		}
		public byte dZ {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			dX = aStream.ReadByte2 ();
			dY = aStream.ReadByte2 ();
			dZ = aStream.ReadByte2 ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteByte(dX);
			aStream.WriteByte (dY);
			aStream.WriteByte (dZ);
		}
	}

	public class EntityLook : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityLook; }
		}
		
		public byte Yaw {
			get;
			set;
		}
		
		public byte Pitch {
			get;
			set;
		}

		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			Yaw = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteByte (Yaw);
			aStream.WriteByte (Pitch);			
		}
	}

	public class EntityLookRelativeMove : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityLookRelativeMove; }
		}

			public byte dX {
			get;
			set;
		}

		public byte dY {
			get;
			set;
		}

		public byte dZ {
			get;
			set;
		}
		
				public byte Yaw {
			get;
			set;
		}

		public byte Pitch {
			get;
			set;
		}

public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			dX = aStream.ReadByte2 ();
			dY = aStream.ReadByte2 ();
			dZ = aStream.ReadByte2 ();
			Yaw = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
		}
		
		public override void WriteToStream(Stream aStream)
		{
			base.WriteToStream(aStream);
			aStream.WriteByte (dX);
			aStream.WriteByte (dY);
			aStream.WriteByte (dZ);
			aStream.WriteByte (Yaw);
			aStream.WriteByte (Pitch);
		}
}

	public class EntityTeleport : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityTeleport; }
		}
		
		public int X { get; set; }

		public int Y { get; set; }

		public int Z { get; set; }
		
		public byte Yaw {
			get;
			set;
		}

		public byte Pitch {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			X = aStream.ReadInt ();
			Y = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Yaw = aStream.ReadByte2 ();
			Pitch = aStream.ReadByte2 ();
		}

		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(X);
			aStream.WriteInt (Y);
			aStream.WriteInt (Z);
			aStream.WriteByte (Yaw);
			aStream.WriteByte (Pitch);
		}
	}

	public class EntityStatus : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityStatus; }
		}
		
		public byte Status {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			Status = aStream.ReadByte2 ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteByte(Status);
		}
	}

	public class AttachEntity : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.AttachEntity; }
		}
		
		public int VehicleID {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			VehicleID = aStream.ReadInt ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt(VehicleID);
		}
	}

	public class EntityMetadata : Entity
	{
		public override PacketType PacketType {
			get { return PacketType.EntityMetadata; }
		}
		
		public MetadataDict Metadata {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			base.ReadFromStream (aStream);
			Metadata = MetadataDict.ReadFromStream (aStream);
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			Metadata.WriteToStream(aStream);
		}
	}

	public class PreChunk : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PreChunk; }
		}
		
		public int X {
			get;
			set;
		}
		public int Z {
			get;
			set;
		}
		
		public bool Mode {
			get;
			set;
		}
		
		public override void ReadFromStream (Stream aStream)
		{
			X = aStream.ReadInt ();
			Z = aStream.ReadInt ();
			Mode = aStream.ReadBoolean ();
		}
		
		public override void WriteToStream (Stream aStream)
		{
			base.WriteToStream (aStream);
			aStream.WriteInt (X);
			aStream.WriteInt (Z);
			aStream.WriteBoolean(Mode);
		}
	}

	public class MapChunk : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.MapChunk; }
		}
	}

	public class MultiBlockChange : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.MultiBlockChange; }
		}
	}

	public class BlockChange : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.BlockChange; }
		}
	}

	public class PlayNoteBlock : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.PlayNoteBlock; }
		}
	}

	public class Explosion : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Explosion; }
		}
	}

	public class OpenWindow : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.OpenWindow; }
		}
	}

	public class CloseWindow : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.CloseWindow; }
		}
	}

	public class WindowClick : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.WindowClick; }
		}
	}

	public class SetSlot : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.SetSlot; }
		}
	}

	public class WindowItems : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.WindowItems; }
		}
	}

	public class UpdateProgressBar : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.UpdateProgressBar; }
		}
	}

	public class Transaction : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.Transaction; }
		}
	}

	public class UpdateSign : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.UpdateSign; }
		}
	}

	public class DisconnectKick : PacketBase
	{
		public override PacketType PacketType {
			get { return PacketType.DisconnectKick; }
		}
	}
	
}

