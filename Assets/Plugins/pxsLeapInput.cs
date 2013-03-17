/******************************************************************************\
* This is a Singleton class that emulates the axis input that most games have *
* Version 0.2
* Made it work with BootCamp Tutorial
*
* Version 0.1
* initially published as part of LeanEnabledCarTutorial
* http://pierresemaan.com/leap-enabling-the-unity3d-car-tutorial/
\******************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>

/// </summary>
public static class pxsLeapInput 
{	
	private enum HandID : int
	{
		Primary		= 0,
		Secondary	= 1
	};
	
	//Create a new leap controller object when you create this static class 
	static Leap.Controller 		m_controller	= null;
	static Leap.Frame			m_Frame			= null;
	static Leap.Hand			m_Hand			= null;
	static int			m_FingersCount			= 0;
	static string m_Errors 			= "";
	
	// constructor called when the first class or member is referenced.
	static pxsLeapInput()
	{
		try
		{
			//Create a new leap controller object when you create this static class 
			m_controller	= new Leap.Controller();
		}
		catch 
		{
			m_Errors = m_Errors + '\r' + '\n'  + "Controller could not be created";
		}
	}

	public static Leap.Frame Frame
	{
		get 		
		{ 
			Update(); return m_Frame; 
		}
	}
	public static int FingersCount
	{
		get 		
		{ 
			Update();
			return m_FingersCount; 
		}
	}
	
	public static Leap.Hand Hand
	{
		
		get 
		{ 
			Update();
			return m_Hand; 
		}
	}
	
	public static string Errors
	{
		get { return m_Errors; }
	}
	
	public static void Update() 
	{	
		m_Hand = null;
		m_Frame = null;
		m_FingersCount = 0;
		if( m_controller != null )
		{
			m_Frame	= m_controller.Frame();
			if (m_Frame != null)
			{
				if (m_Frame.Hands.Count > 0)
				{
					m_Hand = m_Frame.Hands[0];
				}
				
				if (m_Frame.Fingers.Count > 0)
				{
					m_FingersCount = m_Frame.Fingers.Count;
				}
			}
		}
	}
	
	// returns the hand axis scaled from -1 to +1
	public static float GetHandAxis(string axisName)
	{
		float ret = GetHandAxisPrivate(axisName, true);
		return ret;
	}
	
	public static float GetHandAxisRaw(string axisName)
	{
		float ret = GetHandAxisPrivate(axisName, false);
		return ret;
	}
	
	public static bool GetHandGesture(string gestureName)
	{
		// Call Update so you can get the latest frame and hand
		Update();
		bool ret = false;

		Vector3 PalmNormal = new Vector3(0,0,0);
		if (m_Hand != null)
		{

			PalmNormal = m_Hand.PalmNormal.ToUnity();
			

			switch (gestureName)
			{
			case "Fire1":
				ret =  m_FingersCount > 2; 
				break;
			case "Fire2":
				ret =  (PalmNormal.x > 0.5) ; 
				break;	
			case "FireRotation1":
				ret =  (PalmNormal.x < -0.5) ; 
				break;	
			default:
				break;
			}
		}
		return ret;
	}
	
	private static float GetHandAxisPrivate(string axisName, bool scaled)
	{
		// Call Update so you can get the latest frame and hand
		Update();
		float ret = 0.0F;
		if (m_Hand != null)
		{
			Vector3 PalmPosition = new Vector3(0,0,0);
			Vector3 PalmNormal = new Vector3(0,0,0);
			Vector3 PalmDirection = new Vector3(0,0,0);
			if (scaled == true)
			{
				PalmPosition = m_Hand.PalmPosition.ToUnityTranslated();
				PalmNormal = m_Hand.PalmNormal.ToUnity();				
				PalmDirection = m_Hand.Direction.ToUnity();
			}
			else
			{
				PalmPosition = m_Hand.PalmPosition.ToUnity();
				PalmNormal = m_Hand.PalmPosition.ToUnity();
				PalmDirection = m_Hand.Direction.ToUnity();
			}

			switch (axisName)
			{
			case "Horizontal":
				ret =  PalmPosition.x ; 
				break;
			case "Vertical":
				ret =  PalmPosition.y ; 
				break;
			case "Mouse X":
				// rotation is preferred (more usable).  
				// if using rotation for something else, can use Palmdirection by uncommenting below.
				ret = ret = -2 * PalmNormal.x ;
				// ret =  4 * (PalmDirection.x ) + 1.00F ;
				m_Errors = "ret Mouse.x = " + ret.ToString();
				break;
			case "Mouse Y":
				// use z axis as this is the most promounced change when TITLTing a hand
				ret =  2 * PalmNormal.z ;
				// m_Errors = "ret Mouse.Y = " + ret.ToString();
				break;
			
			case "Depth":
				ret =  PalmPosition.z;
				break;
			case "Rotation":
				ret = -2 * PalmNormal.x ;
				break;
			case "Tilt":
				ret = PalmNormal.z ;
				break;
			case "HorizontalDirection":
				ret = PalmDirection.x ;
				break;
			case "VericalDirection":
				ret = PalmDirection.y ;
				break;
			default:
				break;
			}
			if (scaled == true)
				{
					if (ret > 1) {ret = 1;}
					if (ret < -1) {ret = -1;}
				}
	
		}
		else
		{
			// Hand is Null, so return the standard axis
			switch (axisName)
			{
			case "Depth":
				// depth for leap = vertical axis
				ret = Input.GetAxis("Vertical") ;
				break;
			default:
				// return the axis name from input if hand is null and the special cases above do not apply
				ret = Input.GetAxis(axisName) ;
				break;
			}

		}
		return ret;
	}
	
}
