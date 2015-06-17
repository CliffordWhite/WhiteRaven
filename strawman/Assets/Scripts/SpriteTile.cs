using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]

public class SpriteTile : MonoBehaviour {
	SpriteRenderer sprite;

	void Awake () {
		sprite = GetComponent<SpriteRenderer> ();
		Vector2 t_size = new Vector2 (sprite.bounds.size.x / transform.localScale.x,
		                              sprite.bounds.size.y / transform.localScale.y);


		GameObject t_child = new GameObject();
		SpriteRenderer t_childSprite = t_child.AddComponent<SpriteRenderer> ();
		t_childSprite.transform.position = transform.position;
		t_childSprite.sprite = sprite.sprite;
		t_childSprite.sortingOrder = 1;

		GameObject children;
		for (float i = 0, height = sprite.bounds.size.y; i*t_size.y < height; i++) {
			for (float c = 0, width = sprite.bounds.size.x ; c*t_size.x < width; c++) {
				children = Instantiate(t_child) as GameObject;
				Vector3 temp = new Vector3((1.0f-(1.0f/transform.localScale.x))*(-width/2.0f),(1.0f-(1.0f/transform.localScale.y))*(height/2.0f),0);
				children.transform.position = transform.position +temp + (new Vector3(t_size.x*c,-t_size.y*i,0));
				children.transform.parent = transform;
			}
		}

		Destroy (t_child);
		sprite.enabled = false;
	}

}
