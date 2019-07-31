// Generated by github.com/davyxu/tabtoy
// Version: 2.8.5
// DO NOT EDIT!!
using System.Collections.Generic;

namespace table
{
	
	// Defined in table: Base
	public enum SkillTriggerType
	{
		
		
		weatherTrigger = 0, // 天气触发
		
		
		FieldTrigger = 1, // 场地触发
		
		
		StageTrigger = 2, // 阶段触发
		
		
		DynamicTrigger = 3, // 动态触发
		
		
		LongTrigger = 4, // 长度触发
	
	}
	
	// Defined in table: Base
	public enum SkillTriggerContent
	{
		
		
		SunnyDay = 0, // 晴天天气
		
		
		RainyDay = 1, // 雨天天气
		
		
		FoggyDay = 2, // 雾天天气
		
		
		GrassLand = 3, // 草地场地
		
		
		MuddyLand = 4, // 泥地场地
		
		
		SandLand = 5, // 沙地场地
		
		
		StartStage = 6, // 开始阶段
		
		
		EnterTurnStage = 7, // 入弯阶段
		
		
		ExitTurnStage = 8, // 出弯阶段
		
		
		SprintStage = 9, // 冲刺阶段
		
		
		StraightLength = 10, // 直线长度
		
		
		CurveLength = 11, // 弯道长度
		
		
		RaceLength = 12, // 赛程长度
		
		
		Overtake = 13, // 超越对手
		
		
		Overtaken = 14, // 被超越
		
		
		HeadPosition = 15, // 领头位置
		
		
		EndPosition = 16, // 末尾位置
		
		
		BeforeAppoint = 17, // 指定位前
		
		
		AfterAppoint = 18, // 指定位后
		
		
		WearEquipment = 19, // 穿戴装备
		
		
		StraightLine = 20, // 直线阶段
		
		
		CurveLine = 21, // 弯道阶段
	
	}
	
	// Defined in table: Base
	public enum AttributeType
	{
		
		
		Speed = 0, // 速度
		
		
		Endurance = 1, // 耐力
		
		
		Burst = 2, // 爆发
		
		
		Balance = 3, // 平衡
		
		
		AllAttribute = 4, // 全部属性
		
		
		NoneAttribute = 5, // 无
		
		
		ExtraSkill = 6, // 技能
		
		
		PhysicalPower = 7, // 当前体力
	
	}
	
	// Defined in table: Base
	public enum SkillEffectTarget
	{
		
		
		Self = 0, // 自己
		
		
		Enemy = 1, // 单个敌人
	
	}
	
	// Defined in table: Base
	public enum ValueType
	{
		
		
		Integer = 0, // 整数值
		
		
		percentage = 1, // 百分数值
	
	}
	
	// Defined in table: Base
	public enum HorseSkillType
	{
		
		
		NatureSkill = 0, // 性格技能
		
		
		PracticeSkill = 1, // 受训技能
		
		
		EquipSkill = 2, // 装备技能
	
	}
	
	// Defined in table: Base
	public enum CommodityLimitType
	{
		
		
		NoneLimit = 0, // 无限商品
		
		
		DayLimit = 1, // 日限商品
		
		
		WeekLimit = 2, // 周限商品
		
		
		MonthLimit = 3, // 月限商品
		
		
		PermanentLimit = 4, // 永限商品
	
	}
	
	// Defined in table: Base
	public enum GuideHandleType
	{
		
		/// <summary> 
		/// UI气泡
		///info 文本
		///x,y 背景位置 离中心点的偏移
		/// </summary>
		UI气泡 = 0, 
		
		/// <summary> 
		/// 等待进入大厅 无参数
		/// </summary>
		等待进入大厅 = 1, 
		
		/// <summary> 
		/// UI角色
		///x,y 位置 离中心点的偏移
		///x1:-1翻转
		/// </summary>
		UI角色 = 2, 
		
		/// <summary> 
		/// UI镂空
		///x,y 镂空偏移 (三角无效)
		///x1,y1 手偏移
		///rotationZ 手旋转
		///path 镂空目标 
		///hollowType 0方形 1 三角型
		///width  height 镂空宽 高
		/// </summary>
		UI镂空 = 3, 
		
		/// <summary> 
		/// UI遮挡隐藏 无参数
		/// </summary>
		UI遮挡隐藏 = 4, 
		
		/// <summary> 
		/// UI遮挡显示 无参数
		/// </summary>
		UI遮挡显示 = 5, 
		
		/// <summary> 
		/// UI引导按钮
		///info 文本
		///x,y 位置
		/// </summary>
		UI引导按钮 = 6, 
		
		/// <summary> 
		/// 等待UI显示
		///path UI类型名字(预制件名)
		/// </summary>
		等待UI显示 = 7, 
		
		/// <summary> 
		/// 等待点击镂空UI
		/// </summary>
		等待点击镂空UI = 8, 
		
		/// <summary> 
		/// 等待点击引导按钮
		/// </summary>
		等待点击引导按钮 = 9, 
	
	}
	
	

	// Defined in table: Config
	
	public partial class Config
	{
	
		public tabtoy.Logger TableLogger = new tabtoy.Logger();
	
		
		/// <summary> 
		/// Test
		/// </summary>
		public List<TestDefine> Test = new List<TestDefine>(); 
	
	
		#region Index code
	 	Dictionary<int, TestDefine> _TestByID = new Dictionary<int, TestDefine>();
        public TestDefine GetTestByID(int ID, TestDefine def = default(TestDefine))
        {
            TestDefine ret;
            if ( _TestByID.TryGetValue( ID, out ret ) )
            {
                return ret;
            }
			
			if ( def == default(TestDefine) )
			{
				TableLogger.ErrorLine("GetTestByID failed, ID: {0}", ID);
			}

            return def;
        }
		
	
		#endregion
		#region Deserialize code
		
		static tabtoy.DeserializeHandler<Config> ConfigDeserializeHandler = new tabtoy.DeserializeHandler<Config>(Deserialize);
		public static void Deserialize( Config ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0xa0000:
                	{
						ins.Test.Add( reader.ReadStruct<TestDefine>(TestDefineDeserializeHandler) );
                	}
                	break; 
                }
             }

			
			// Build Test Index
			for( int i = 0;i< ins.Test.Count;i++)
			{
				var element = ins.Test[i];
				
				ins._TestByID.Add(element.ID, element);
				
			}
			
		}
		static tabtoy.DeserializeHandler<ItemPackGroup> ItemPackGroupDeserializeHandler = new tabtoy.DeserializeHandler<ItemPackGroup>(Deserialize);
		public static void Deserialize( ItemPackGroup ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.ItemId = reader.ReadInt32();
                	}
                	break; 
                	case 0x20001:
                	{
						ins.Count = reader.ReadInt64();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.Weight = reader.ReadInt32();
                	}
                	break; 
                	case 0x10003:
                	{
						ins.GroupId = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<RankingAward> RankingAwardDeserializeHandler = new tabtoy.DeserializeHandler<RankingAward>(Deserialize);
		public static void Deserialize( RankingAward ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.StartRanking = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.EndRanking = reader.ReadInt32();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.ItemPackId = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<DropShow> DropShowDeserializeHandler = new tabtoy.DeserializeHandler<DropShow>(Deserialize);
		public static void Deserialize( DropShow ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.DropID = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.DropNumber = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<GrowRandom> GrowRandomDeserializeHandler = new tabtoy.DeserializeHandler<GrowRandom>(Deserialize);
		public static void Deserialize( GrowRandom ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.GrowType = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.GrowRate = reader.ReadInt32();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.GrowMin = reader.ReadInt32();
                	}
                	break; 
                	case 0x10003:
                	{
						ins.GrowMax = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<EquipmentExtend> EquipmentExtendDeserializeHandler = new tabtoy.DeserializeHandler<EquipmentExtend>(Deserialize);
		public static void Deserialize( EquipmentExtend ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.ExtendType = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.ExtendRate = reader.ReadInt32();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.AttributeMin = reader.ReadInt32();
                	}
                	break; 
                	case 0x10003:
                	{
						ins.AttributeMax = reader.ReadInt32();
                	}
                	break; 
                	case 0x10004:
                	{
						ins.SkillID = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<MapVector> MapVectorDeserializeHandler = new tabtoy.DeserializeHandler<MapVector>(Deserialize);
		public static void Deserialize( MapVector ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.VectorX = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.VectorY = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<EquipmentMaterialList> EquipmentMaterialListDeserializeHandler = new tabtoy.DeserializeHandler<EquipmentMaterialList>(Deserialize);
		public static void Deserialize( EquipmentMaterialList ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.MaterialID = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.MaterialNumber = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<GuideHandle> GuideHandleDeserializeHandler = new tabtoy.DeserializeHandler<GuideHandle>(Deserialize);
		public static void Deserialize( GuideHandle ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.type = reader.ReadInt32();
                	}
                	break; 
                	case 0x60001:
                	{
						ins.info = reader.ReadString();
                	}
                	break; 
                	case 0x50002:
                	{
						ins.x = reader.ReadFloat();
                	}
                	break; 
                	case 0x50003:
                	{
						ins.y = reader.ReadFloat();
                	}
                	break; 
                	case 0x50004:
                	{
						ins.x1 = reader.ReadFloat();
                	}
                	break; 
                	case 0x50005:
                	{
						ins.y1 = reader.ReadFloat();
                	}
                	break; 
                	case 0x50006:
                	{
						ins.rotationZ = reader.ReadFloat();
                	}
                	break; 
                	case 0x10007:
                	{
						ins.hollowType = reader.ReadInt32();
                	}
                	break; 
                	case 0x10008:
                	{
						ins.width = reader.ReadInt32();
                	}
                	break; 
                	case 0x10009:
                	{
						ins.height = reader.ReadInt32();
                	}
                	break; 
                	case 0x6000a:
                	{
						ins.path = reader.ReadString();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<GameAttributeDisplay> GameAttributeDisplayDeserializeHandler = new tabtoy.DeserializeHandler<GameAttributeDisplay>(Deserialize);
		public static void Deserialize( GameAttributeDisplay ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.属性类型 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.属性上限 = reader.ReadInt32();
                	}
                	break; 
                	case 0x10002:
                	{
						ins.属性下限 = reader.ReadInt32();
                	}
                	break; 
                }
             }

			
		}
		static tabtoy.DeserializeHandler<TestDefine> TestDefineDeserializeHandler = new tabtoy.DeserializeHandler<TestDefine>(Deserialize);
		public static void Deserialize( TestDefine ins, tabtoy.DataReader reader )
		{
 			int tag = -1;
            while ( -1 != (tag = reader.ReadTag()))
            {
                switch (tag)
                { 
                	case 0x10000:
                	{
						ins.ID = reader.ReadInt32();
                	}
                	break; 
                	case 0x10001:
                	{
						ins.TestInt = reader.ReadInt32();
                	}
                	break; 
                	case 0x20002:
                	{
						ins.TestLong = reader.ReadInt64();
                	}
                	break; 
                	case 0x50003:
                	{
						ins.TestFloat = reader.ReadFloat();
                	}
                	break; 
                	case 0x60004:
                	{
						ins.TestString = reader.ReadString();
                	}
                	break; 
                	case 0x10005:
                	{
						ins.TestIntArr.Add( reader.ReadInt32() );
                	}
                	break; 
                	case 0x50006:
                	{
						ins.TestFloat2 = reader.ReadFloat();
                	}
                	break; 
                	case 0x60007:
                	{
						ins.TestStringArr.Add( reader.ReadString() );
                	}
                	break; 
                	case 0x80008:
                	{
						ins.TestEnum0 = reader.ReadEnum<SkillTriggerType>();
                	}
                	break; 
                	case 0x80009:
                	{
						ins.TestEnum1 = reader.ReadEnum<SkillTriggerType>();
                	}
                	break; 
                	case 0x9000a:
                	{
						ins.TestClass = reader.ReadStruct<DropShow>(DropShowDeserializeHandler);
                	}
                	break; 
                	case 0x6000b:
                	{
						ins.Name = reader.ReadString();
                	}
                	break; 
                }
             }

			
		}
		#endregion
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class ItemPackGroup
	{
	
		
		
		public int ItemId = 0; // 物品ID
		
		
		public long Count = 0; // 物品数量
		
		
		public int Weight = 0; // 物品权重
		
		
		public int GroupId = 0; // 物品分组
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class RankingAward
	{
	
		
		/// <summary> 
		/// 开始排名
		/// </summary>
		public int StartRanking = 0; // 开始排名
		
		/// <summary> 
		/// 结束排名
		/// </summary>
		public int EndRanking = 0; // 结束排名
		
		/// <summary> 
		/// 物品包ID
		/// </summary>
		public int ItemPackId = 0; // 物品包ID
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class DropShow
	{
	
		
		
		public int DropID = 0; // 展示ID
		
		
		public int DropNumber = 0; // 展示数量
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class GrowRandom
	{
	
		
		
		public int GrowType = 0; // 成长类型
		
		
		public int GrowRate = 0; // 类型权重
		
		
		public int GrowMin = 0; // 最低成长
		
		
		public int GrowMax = 0; // 最高成长
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class EquipmentExtend
	{
	
		
		
		public int ExtendType = 0; // 装备附加类型
		
		
		public int ExtendRate = 0; // 装备附加权重
		
		
		public int AttributeMin = 0; // 附加最低属性
		
		
		public int AttributeMax = 0; // 附加最高属性
		
		
		public int SkillID = 0; // 附加技能ID
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class MapVector
	{
	
		
		
		public int VectorX = 0; // 坐标X
		
		
		public int VectorY = 0; // 坐标Y
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class EquipmentMaterialList
	{
	
		
		
		public int MaterialID = 0; // 材料ID
		
		
		public int MaterialNumber = 0; // 材料数量
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class GuideHandle
	{
	
		
		/// <summary> 
		/// 对应GuideHandleType,
		/// </summary>
		public int type = -1; 
		
		/// <summary> 
		/// 文本
		/// </summary>
		public string info = ""; 
		
		/// <summary> 
		/// 坐标,偏移
		/// </summary>
		public float x = 0f; 
		
		
		public float y = 0f; 
		
		/// <summary> 
		/// 手偏移
		/// </summary>
		public float x1 = 0f; 
		
		
		public float y1 = 0f; 
		
		
		public float rotationZ = 0f; 
		
		/// <summary> 
		/// 镂空类型 0方形 1 三角型
		/// </summary>
		public int hollowType = 0; 
		
		/// <summary> 
		/// 宽
		/// </summary>
		public int width = 0; 
		
		/// <summary> 
		/// 高
		/// </summary>
		public int height = 0; 
		
		/// <summary> 
		/// ui路径,ui名字
		/// </summary>
		public string path = ""; 
	
	

	} 

	// Defined in table: Base
	[System.Serializable]
	public partial class GameAttributeDisplay
	{
	
		
		/// <summary> 
		/// 属性1速度2耐力3爆发4平衡
		/// </summary>
		public int 属性类型 = 0; // 类型
		
		
		public int 属性上限 = 0; // 上限
		
		
		public int 属性下限 = 0; // 下限
	
	

	} 

	// Defined in table: Test
	[System.Serializable]
	public partial class TestDefine
	{
	
		
		/// <summary> 
		/// 血统ID
		/// </summary>
		public int ID = 0; 
		
		/// <summary> 
		/// Test int
		/// </summary>
		public int TestInt = 0; 
		
		/// <summary> 
		/// TestLong
		/// </summary>
		public long TestLong = 0; 
		
		/// <summary> 
		/// TestFloat
		/// </summary>
		public float TestFloat = 0f; 
		
		/// <summary> 
		/// TestString
		/// </summary>
		public string TestString = ""; 
		
		/// <summary> 
		/// TestIntArr
		/// </summary>
		public List<int> TestIntArr = new List<int>(); 
		
		/// <summary> 
		/// TestFloat2
		/// </summary>
		public float TestFloat2 = 0f; 
		
		/// <summary> 
		/// 上限速度
		/// </summary>
		public List<string> TestStringArr = new List<string>(); 
		
		/// <summary> 
		/// TestEnum
		/// </summary>
		public SkillTriggerType TestEnum0 = SkillTriggerType.weatherTrigger; 
		
		/// <summary> 
		/// TestEnum
		/// </summary>
		public SkillTriggerType TestEnum1 = SkillTriggerType.weatherTrigger; 
		
		/// <summary> 
		/// TestClass
		/// </summary>
		public DropShow TestClass = new DropShow(); 
		
		
		public string Name = ""; 
	
	

	} 

}
