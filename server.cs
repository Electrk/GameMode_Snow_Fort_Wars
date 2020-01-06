datablock TriggerData (BoundaryTriggerData)
{
	tickPeriodMS = 100;
};

function BoundaryTriggerData::onLeaveTrigger ( %data, %trigger, %object )
{
	if ( %object.getClassName () $= "Player"  &&  isObject (%client = %object.client) )
	{
		if ( isObject (%client.miniGame) )
		{
			%client.instantRespawn ();
		}
	}
}

package GameMode_Snow_Fort_Wars
{
	function GameModeInitialResetCheck ()
	{
		Parent::GameModeInitialResetCheck ();

		%count = PlayerDropPoints.getCount ();

		for ( %i = 0;  %i < %count;  %i++ )
		{
			%spawnSphere = PlayerDropPoints.getObject (%i);

			%spawnSphere.radius   = 32;
			%spawnSphere.position = "-32 32 0";
		}

		%boundary = new Trigger (RespawnBoundary)
		{
			dataBlock  = BoundaryTriggerData;
			position   = "-64 64 0";
			rotation   = "1 0 0 0";
			polyhedron = "0 0 0 1 0 0 0 -1 0 0 0 1";
			scale      = "64 64 16";
		};
		MissionGroup.add (%boundary);

		echo ("Creating snow brick grid...");

		%before = getRealTime ();
		BuildableSnow_CreateGrid (128, 128, 6);

		echo ("Snow brick grid created in " @ (getRealTime () - %before) @ "ms");
	}

	function serverCmdLight ( %client )
	{
		if ( !isObject (%client.miniGame) )
		{
			Parent::serverCmdLight (%client);
		}
	}

	function BuildableSnow_CreateSnowBrick ( %x, %y, %z )
	{
		%brick = Parent::BuildableSnow_CreateSnowBrick (%x, %y, %z);

		if ( %z > 0 )
		{
			%brick.setSnowVertices (0, 0, 0, 0);
		}

		return %brick;
	}

	function serverCmdTeamMessageSent () {}
};
activatePackage (GameMode_Snow_Fort_Wars);
