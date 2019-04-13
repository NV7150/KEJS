using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KendoState {
	GAME,
	HIT,
	IPPON
}

public class KendoManager : MonoBehaviour {
	
	private KendoState currState;
	
	public KendoState CurrState {
		get { return currState; }
	}
	
	public delegate void GameProcess();

	public delegate void PlayerProcess(int player);

	public event PlayerProcess OnHit;
	public event PlayerProcess OnIppon;
	public event GameProcess OnBack;

	public void changeState(KendoState state,int player) {
		if (state != KendoState.GAME && (player <= 0 && 2 < player)) {
			throw new ArgumentException("Invalid player value " + player);
		}
		
		switch (state) {
			case KendoState.GAME:
				if(OnBack != null)
					OnBack();
				break;
			case KendoState.HIT:
				if (OnHit != null)
					OnHit(player);
				break;
			case KendoState.IPPON:
				if (OnIppon != null)
					OnIppon(player);
				break;
		}

		currState = state;
	}
}
