using Godot;
using System;

public class Player2 : KinematicBody2D
{
    // Called when the node enters the scene tree for the first time.

     [Export]
     public int movementspeed;

     [Export]
     public float jumpHeight;

     [Export]
     public float timeJumpApex;

     private float jumpForce;
     private int hp;
     private float gravity;
     private bool isGrounded=true;

     public AnimatedSprite an;
     private AnimationTree tree;
     private AnimationNodeStateMachinePlayback FSM;

     private RayCast2D rayCastGround;

     private RayCast2D rayCastAttack;
     private Vector2 velocity;
     private Enemy e;
     private Vector2 knockback=Vector2.Zero;

     public Global g;
    public override void _Ready()
    {
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSM = (AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSM.Start("Idle");
        velocity=new Vector2();
        g=GetNode<Global>("/root/Global");
        rayCastGround=this.GetNode<RayCast2D>("RayCastGround");
        rayCastAttack=this.GetNode<RayCast2D>("RayCastAttack");
        hp=g.p.Health;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
        gravity=(2*jumpHeight)/Mathf.Pow(timeJumpApex,2);
        jumpForce=gravity*timeJumpApex;
        velocity.y+=gravity*delta;
        if(Input.IsKeyPressed((int)KeyList.D)){

            this.Position+=Vector2.Right*movementspeed*delta;
            an.FlipH=false;
            FSM.Travel("Run");
            rayCastAttack.CastTo=new Vector2(36.715f,0);
           
        }
        else if(Input.IsKeyPressed((int)KeyList.A)){
            this.Position+=Vector2.Left*movementspeed*delta;
            an.FlipH=true;
            FSM.Travel("Run");
            rayCastAttack.CastTo=new Vector2(-36.715f,0);
       
        }
        else
            FSM.Travel("Idle"); 

        if(Input.IsActionJustPressed("Jumpp")){
            if(isGrounded){
                velocity.y=-jumpForce;
                isGrounded=false;
            }
        }
        velocity=MoveAndSlide(velocity,Vector2.Up);
        PlayJumping();
        Attacking();
        if(hp>g.p.Health){
            hp=g.p.Health;
        }
        knockback=knockback.MoveToward(Vector2.Zero,200*delta);
        knockback=MoveAndSlide(knockback);
  }
    private void PlayJumping(){
        if(rayCastGround.IsColliding())
            isGrounded=true;
        else{
            isGrounded=false;
            if(velocity.y<0)
                FSM.Travel("Jump");
            else
                FSM.Travel("Fall");
        }
    }
    private void Attacking(){
        if(Input.IsActionJustPressed("Attack") && isGrounded){
             FSM.Travel("Attack");
             AttackColliding();
        }
    }
    private void _on_HurtBox_body_entered(Node body){

        if(!(body is Enemy))
            return;
        Enemy e=(Enemy)body;
        Hurt();
        FSM.Travel("TakeHit");
        knockback=e.knockback_dir*300;
    }
    private void Hurt(){
        g.p.Health--;
        g.LoseHeart();
        if( g.p.Health<=0)
            CallDeferred("free");
    }
    private void _on_EnemyHit_body_entered(Node body){
        if(!(body is Enemy))
            return;
        Enemy e=(Enemy)body;
        g.p.setCritDmg();
        e.Hurt(g.p.DamageDeal);        
    }
    
    private void AttackColliding(){
        if(rayCastAttack.IsColliding()){
            Enemy e=(Enemy)rayCastAttack.GetCollider();
            g.p.setCritDmg();
            e.Hurt(g.p.DamageDeal);  
        }
    }
}

