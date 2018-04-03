using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RobeTest1 {

	[Test]
	public void RobeTest1SimplePasses() {
		// Use the Assert class to test conditions.
	}
	[Test]
	public void RopeActiveInitTest(){
		
		RopeController Rope = new RopeController();

		Assert.AreEqual(false, Rope.getRA());

	}
	[Test]
	public void RopeActiveSetRATest(){

		RopeController Rope = new RopeController();
		Rope.setRA (true);

		Assert.AreEqual(true, Rope.getRA());

	}

	//ask how can i connect rope to the wall
	[UnityTest]
	public IEnumerator SRTEST1()
	{
		GameObject rope = new GameObject();
		RopeController r = new RopeController();
		rope.AddComponent(typeof(RopeController));
		rope.AddComponent (typeof(DistanceJoint2D));


		RopeController ropeC = rope.GetComponent<RopeController>();
//		ropeC.ropeActive = true;
		ropeC.ShootRopeBypassVectors();


		Assert.AreEqual(true, ropeC.ropeActive);


		yield return null;

	}
	[UnityTest]
	public IEnumerator SRTEST2()
	{
		GameObject rope = new GameObject();
		RopeController r = new RopeController();
		rope.AddComponent(typeof(RopeController));
		rope.AddComponent (typeof(DistanceJoint2D));


		RopeController ropeC = rope.GetComponent<RopeController>();
	    ropeC.ropeActive = false;
		ropeC.ShootRopeBypassHit();


		Assert.AreEqual(true, ropeC.ropeActive);


		yield return null;

	}
	[UnityTest]
	public IEnumerator RRTEST1()
	{   
		GameObject rope = new GameObject();
		RopeController r = new RopeController();
		rope.AddComponent(typeof(RopeController));
		rope.AddComponent (typeof(LineRenderer));

		RopeController ropeC = rope.GetComponent<RopeController>();
		ropeC.ropeActive = false;
		ropeC.setRope ();
		ropeC.RenderRopeT();


		Assert.AreEqual(true, ropeC.ropeActive);


		yield return null;

	}
	[UnityTest]
	public IEnumerator RRTEST2()
	{   
		GameObject rope = new GameObject();
		RopeController r = new RopeController();
		rope.AddComponent(typeof(RopeController));
		rope.AddComponent (typeof(LineRenderer));

		RopeController ropeC = rope.GetComponent<RopeController>();
		ropeC.ropeActive = true;
		ropeC.RenderRopeT();


		Assert.AreEqual(false, ropeC.ropeActive);


		yield return null;

	}



	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator RobeTest1WithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
