
namespace Burdle
{
	public partial class Platform: BurdleModelEntity
	{

		public enum PlatformTypes { 
			p1x1,
			p1x1Thin,
			p2x1,
			p2x1Thin,
			p2x2,
			p2x2Thin,
		}

		public static readonly string[] PlatformTypeModels = {
			"degg/models/simple/platform_1x1.vmdl",
			"degg/models/simple/platform_1x1_thin.vmdl",
			"degg/models/simple/platform_2x1.vmdl",
			"degg/models/simple/platform_2x1_thin.vmdl",
			"degg/models/simple/platform_2x2.vmdl",
			"degg/models/simple/platform_2x2_thin.vmdl"
		};

		public override void Spawn()
		{
			base.Spawn();
			SetType( PlatformTypes.p1x1);
		}

		public void SetType( PlatformTypes platformType )
		{
			SetModelAndPhysics( PlatformTypeModels[(int)platformType] );
		}
	}
}
