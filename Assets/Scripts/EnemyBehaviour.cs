using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class EnemyBehaviour: MonoBehaviour
{
    private Transform playerTransform;
    private Transform cachedTransform;
    private Rigidbody2D rb;
    private Transform platform;

    private void Start()
    {
        playerTransform = FindObjectOfType<MainCharacter>().transform;
        cachedTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        platform = GameObject.Find("Platform").gameObject.transform;
    }

    private const float whiteSpaceLeft = 300;
    private const float whiteSpaceRight = 100;
    public bool IsPlayerNear () => cachedTransform.position.x - whiteSpaceLeft<= playerTransform.position.x 
                                                    && cachedTransform.position.x + whiteSpaceRight >= playerTransform.position.x;

    private void Update()
    {
        var leftBoreder = platform.position.x - platform.localScale.x / 2;
        var rightBoreder = platform.position.x + platform.localScale.x / 2;
        if (IsPlayerNear() && cachedTransform.position.x > leftBoreder
            && cachedTransform.position.x < rightBoreder)
        {
            var directionX = cachedTransform.position.x > playerTransform.position.x ? -1 : 1;
            rb.velocity = new Vector2(speedX * directionX, 0);
        }
        if (IsPlayerNear() && rb.velocity.x != 0
            && Math.Abs(playerTransform.position.x - cachedTransform.position.x) < 2)
            rb.velocity = new Vector2(0, 0);
    }

    private const float speedX = 10f;
}
