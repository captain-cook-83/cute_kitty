using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAIProperties : MonoBehaviour {

	/**
	 * 性格属性
	 */

	private float unruliness = 100F;				//无法无天

	private float cleverness = 100F;				//聪明伶俐

	private float affinityDegree = 100F;			//亲和度

	private float lively = 100F;					//活泼好动

	public float Unruliness {
		get { return unruliness; }
		set { unruliness = value; }
	}

	public float Cleverness {
		get { return cleverness; }
		set { cleverness = value; }
	}

	public float AffinityDegree {
		get { return affinityDegree; }
		set { affinityDegree = value; }
	}

	public float Lively {
		get { return lively; }
		set { lively = value; }
	}

	/**
	 * 善变属性
	 */

	private float happiness = 100F;				//开心值（健康程度+，饱足程度+）

	private float physicalPower = 100F;			//体力值（健康程度+，饱足程度+）

	private float satiateDegree = 100F;			//饱足程度

	private float healthDegree = 100F;			//健康程度

	public float Happiness {
		get { return happiness; }
		set { happiness = value; }
	}

	public float PhysicalPower {
		get { return physicalPower; }
		set { physicalPower = value; }
	}

	public float SatiateDegree {
		get { return satiateDegree; }
		set { satiateDegree = value; }
	}

	public float HealthDegree {
		get { return healthDegree; }
		set { healthDegree = value; }
	}

	/**
	 * 额外欲望
	 */

	private float needToAccompany = 0F;		//陪伴（亲和度+；开心值-）

	private float wantToExplore = 0F;		//探索（活泼好动+；体力+）

	private float wantToPlay = 0F;			//玩耍（无法无天+，活泼好动+；体力+，开心+）
		
	private float wantToTrain = 0F;			//锻炼（活泼好动+；体力+，开心+）
}
