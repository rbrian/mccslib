using System;
using System.Collections.Generic;
using System.IO;

namespace mccslib
{
	public class MetadataDict : Dictionary<byte, Metadata>
	{		
		public static MetadataDict ReadFromStream (Stream aStream)
		{
			byte x;
			MetadataDict result = new MetadataDict ();
			while ((x = aStream.ReadByte2()) != 127) {
				byte xIndex = (byte)(x & 0x1F);
				switch (x >> 5) {
					case 0 :
						result [xIndex] = new MetadataByte () { Value = aStream.ReadByte2() };
						break;
					default: 
						throw new Exception ("Unrecognized metadata type " + (x >> 5).ToString ());
				}
			}

			return result;
		}
		
		public void WriteToStream (Stream aStream)
		{
			foreach (var lEntry in this.Values) {
				byte lIndex = (byte)((lEntry.Key << 5) + lEntry.Index);
				aStream.WriteByte (lIndex);
				lEntry.WriteToStream(aStream);
			}
			aStream.WriteByte (127);
		}
	}
	
	public abstract class Metadata
	{
		public abstract byte Key { get; }
		public byte Index {
			get;
			set;
		}
		
		public Metadata ()
		{
		}
		
		public abstract void WriteToStream (Stream aStream);
	}

	public class MetadataByte : Metadata
	{
		public override byte Key {
			get {
				return 0;
			}
		}
		public byte Value {
			get;
			set;
		}
		
		public override void WriteToStream(Stream aStream)
		{
			aStream.WriteByte(Value);
		}
	}

	public class MetadataShort : Metadata
	{
		public override byte Key {
			get {
				return 1;
			}
		}
		public short Value {
			get;
			set;
		}
		
		public override void WriteToStream (Stream aStream)
		{
			aStream.WriteShort (Value);
		}
	}
	
		public class MetadataInt : Metadata
	{
		public override byte Key {
			get {
				return 2;
			}
		}
		
		public int Value {
			get;
			set;
		}
		
		public override void WriteToStream(Stream aStream)
		{
			aStream.WriteInt(Value);
		}
	}
	
		public class MetadataFloat: Metadata
	{
		public override byte Key {
			get {
				return 3;
			}
		}
		public float Value {
			get;
			set;
		}
		
		public override void WriteToStream(Stream aStream)
		{
			aStream.WriteFloat(Value);
		}
	}

}


