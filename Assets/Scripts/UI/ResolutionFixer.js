/*  If using for Mobile devices, click on ChangeX in editor,
	else If GameObject is Rotated, then click on ChangeXRotated
	And drag MainCamera in MainCamera field in editor */

#pragma strict
public var WorkingXorRatio:int;
public var WorkingYorRatio:int;
public var ChangeX:boolean;
public var ChangeY:boolean;
public var ChangeXRotated:boolean;
public var ChangeYRotated:boolean;
public var MainCamera:Transform;

private var HeightStore:int;
private var WidthStore:int;
private var ratiox:float;
private var ratioy:float;
private var xdistance:float;
private var ydistance:float;
private var newxdistance:float;


function Start () 
{
	HeightStore=Screen.height;
	WidthStore=Screen.width;

 	ratiox=Screen.width*WorkingYorRatio;
 	ratioy=Screen.height*WorkingXorRatio;

	if(ChangeX)
	{
		transform.localScale.x=transform.localScale.x*ratiox/ratioy;
	 	xdistance=this.transform.position.x-MainCamera.position.x;
		newxdistance=xdistance*(ratiox/ratioy);
	 	this.transform.position.x+=newxdistance-xdistance;
	}
	if(ChangeY)
	{
		transform.localScale.y=transform.localScale.y*ratioy/ratiox;
		xdistance=this.transform.position.y-MainCamera.position.y;
		newxdistance=xdistance*(ratioy/ratiox);
	 	this.transform.position.y+=newxdistance-xdistance;
	}
	if(ChangeXRotated)
	{
		transform.localScale.y=transform.localScale.y*ratiox/ratioy;
		xdistance=this.transform.position.x-MainCamera.position.x;
		newxdistance=xdistance*(ratiox/ratioy);
	 	this.transform.position.x+=newxdistance-xdistance;
	}
	 if(ChangeYRotated)
	{
		transform.localScale.x=transform.localScale.x*ratioy/ratiox;
		xdistance=this.transform.position.y-MainCamera.position.y;
		newxdistance=xdistance*(ratioy/ratiox);
	 	this.transform.position.y+=newxdistance-xdistance;
	}
}