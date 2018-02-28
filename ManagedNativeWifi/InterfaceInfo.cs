using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ManagedNativeWifi.Win32.NativeMethod;

namespace ManagedNativeWifi
{
	/// <summary>
	/// Wireless interface information
	/// </summary>
	public class InterfaceInfo
	{
		/// <summary>
		/// Interface ID
		/// </summary>
		public Guid Id { get; }

		/// <summary>
		/// Interface description
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Interface state
		/// </summary>
		public InterfaceState State { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		public InterfaceInfo(Guid id, string description, InterfaceState state)
		{
			this.Id = id;
			this.Description = description;
			this.State = state;
		}

		internal InterfaceInfo(WLAN_INTERFACE_INFO info)
		{
			Id = info.InterfaceGuid;
			Description = info.strInterfaceDescription;
			State = InterfaceStateConverter.Convert(info.isState);
		}
	}

	public class InterfaceInfoEx : InterfaceInfo
	{
		public ConnectionMode ConnectionMode { get; }
		public string ProfileName { get; }
		public AssociationAttributes AssociationAttributes { get; }
		public SecurityAttributes SecurityAttributes { get; }
		/// <summary>
		/// Constructor
		/// </summary>
		public InterfaceInfoEx(Guid id, string description, InterfaceState state) : base(id, description, state)
		{
		}

		internal InterfaceInfoEx(WLAN_INTERFACE_INFO info, WLAN_CONNECTION_ATTRIBUTES attributes) : base(info)
		{
			ConnectionMode = (ConnectionMode) attributes.wlanConnectionMode;
			ProfileName = attributes.strProfileName;
			AssociationAttributes = new AssociationAttributes(attributes.wlanAssociationAttributes);
			SecurityAttributes = new SecurityAttributes(attributes.wlanSecurityAttributes);
		}
	}

	public class AssociationAttributes
	{
		private readonly WLAN_ASSOCIATION_ATTRIBUTES _attributes;

		internal AssociationAttributes(WLAN_ASSOCIATION_ATTRIBUTES attributes)
		{
			_attributes = attributes;
		}
	}

	public class SecurityAttributes
	{
		private readonly WLAN_SECURITY_ATTRIBUTES _attributes;

		public bool SecurityEnabled { get; }
		public bool OneXEnabled { get; }
		public AuthAlgorithm AuthAlgorithm { get; }
		public CipherAlgorithm CipherAlgorithm { get; }

		internal SecurityAttributes(WLAN_SECURITY_ATTRIBUTES attributes)
		{
			_attributes = attributes;
			SecurityEnabled = attributes.bSecurityEnabled;
			OneXEnabled = attributes.bOneXEnabled;
			AuthAlgorithm = (AuthAlgorithm) attributes.dot11AuthAlgorithm;
			CipherAlgorithm = (CipherAlgorithm) attributes.dot11CipherAlgorithm;
		}
	}

	
	public enum AuthAlgorithm : uint
	{
		Alg80211Open = 1,
		Alg80211SharedKey = 2,
		Wpa = 3,
		WpaPsk = 4,
		WpaNone = 5,
		Rsna = 6,
		RsnaPsk = 7,
		IhvStart = 0x80000000,
		IhvEnd = 0xffffffff
	}

	public enum CipherAlgorithm : uint
	{
		None = 0x00,
		Wep40 = 0x01,
		Tkip = 0x02,
		Ccmp = 0x04,
		Wep104 = 0x05,
		WpaUseGroup = 0x100,
		RsnUseGroup = 0x100,
		Wep = 0x101,
		IhvStart = 0x80000000,
		IhvEnd = 0xffffffff
	}
	
	public enum ConnectionMode
	{
		Profile,
		TemporaryProfile,
		DiscoverySecure,
		DiscoveryUnsecure,
		Auto,
		Invalid
	}
}