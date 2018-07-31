using ETModel;
namespace ETModel
{
	[Message(FrameOpcode.OneFrameMessage)]
	public partial class OneFrameMessage : IActorMessage {}

	[Message(FrameOpcode.FrameMessage)]
	public partial class FrameMessage : IActorMessage {}

	[Message(FrameOpcode.FSPParam)]
	public partial class FSPParam : IMessage {}

	[Message(FrameOpcode.FSPVKey)]
	public partial class FSPVKey : IMessage {}

	[Message(FrameOpcode.FSPDataC2S)]
	public partial class FSPDataC2S : IActorMessage {}

	[Message(FrameOpcode.FSPDataS2C)]
	public partial class FSPDataS2C : IActorMessage {}

	[Message(FrameOpcode.FSPFrame)]
	public partial class FSPFrame : IMessage {}

}
namespace ETModel
{
	public static partial class FrameOpcode
	{
		 public const ushort OneFrameMessage = 11;
		 public const ushort FrameMessage = 12;
		 public const ushort FSPParam = 13;
		 public const ushort FSPVKey = 14;
		 public const ushort FSPDataC2S = 15;
		 public const ushort FSPDataS2C = 16;
		 public const ushort FSPFrame = 17;
	}
}
