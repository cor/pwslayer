using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggleSoundController : MonoBehaviour {

	public bool inventoryIsOpen = true;
	public AudioClip openClip;
	public AudioClip closeClip;
	private AudioSource audioSource;


	public void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySound() {
		audioSource.PlayOneShot( inventoryIsOpen ? closeClip : openClip);
		inventoryIsOpen = !inventoryIsOpen;
	}
}
