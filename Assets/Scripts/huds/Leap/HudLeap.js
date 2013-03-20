#pragma strict
#pragma implicit
#pragma downcast

class HudLeap extends MonoBehaviour
{
	public var quadrant : Texture2D[];
	public var monitor : Texture2D;
	public var dot : Texture2D;
	private var inQuadrant : int = 0;
	private var mouseX : float = 0;
	private var mouseY : float = 0;
	private var auxColor : Color;
	private var cColor : Color;
	private var startCornerMonitor : Vector2;
	private var startCornerNav : Vector2;
	
	function Start()
	{
	}
	
	function OnGUI()
	{
		auxColor = cColor = GUI.color;
		// top right corner
		startCornerMonitor = new Vector2(Screen.width, 0) - new Vector2(monitor.width, 0);
		startCornerNav = new Vector2(Screen.width, 0) - new Vector2(monitor.width, 0) + new Vector2(0, monitor.height + 2) ;

		ShowQuadrant(quadrant[inQuadrant]);
		ShowMonitor();
		
		GUI.color = cColor;
	}
	

	function Update()
	{
		var x : float = pxsLeapInput.GetHandAxisStep("Horizontal");
		var z : float = pxsLeapInput.GetHandAxisStep("Depth");
		
		mouseX = pxsLeapInput.GetHandAxis("Mouse X");
		mouseY = pxsLeapInput.GetHandAxis("Mouse Y");

		if (z == 1.0F)
		{
				if (x == -1.0F)
				{
					inQuadrant = 0;
				}
				else if (x == 0F)
				{
					inQuadrant = 1;
				}
				else
				{
					inQuadrant = 2;
				}			
		}
		else if (z == 0F)
		{
				if (x == -1.0F)
				{
					inQuadrant = 3;
				}
				else if (x == 0F)
				{
					inQuadrant = 4;
				}
				else
				{
					inQuadrant = 5;
				}	
		}
		else
		{
				if (x == -1.0F)
				{
					inQuadrant = 6;
				}
				else if (x == 0F)
				{
					inQuadrant = 7;
				}
				else
				{
					inQuadrant = 8;
				}	
		}
		// print("x = " + x.ToString() + ", z = " + z.ToString() + "' inQuadrant = " + inQuadrant.ToString());
	}
	
	function ShowMonitor()
	{
		GUI.color = auxColor;
		var x = startCornerMonitor.x ;
		var y = startCornerMonitor.y ;
		var graphicRect = new Rect(x, y, monitor.width, monitor.height);
		GUI.DrawTexture(graphicRect, monitor);
		
		x = startCornerMonitor.x + (monitor.width /2) + ((monitor.width - dot.width) * mouseX/2);
		y = startCornerMonitor.y + (monitor.height /2) + ((monitor.height - dot.height) * mouseY/2) ;
		graphicRect = new Rect(x, y, dot.width, dot.height);
		GUI.DrawTexture(graphicRect, dot);
	
	}	
	
	
	
	function ShowQuadrant(aQuadrant : Texture2D)
	{
		GUI.color = auxColor;
		var x = startCornerNav.x ;
		var y = startCornerNav.y ;
		var graphicRect = new Rect(x, y, aQuadrant.width, aQuadrant.height);
		GUI.DrawTexture(graphicRect, aQuadrant);
	}
}