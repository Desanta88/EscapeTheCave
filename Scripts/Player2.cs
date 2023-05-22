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

     public float jumpForce;

     private float gravity;
     private bool isGrounded=true;

     public PlayerC p;

     private AnimatedSprite an;
     private AnimationTree tree;
     Vector2 v;
     AnimationNodeStateMachinePlayback FSM;
     Vector2 velocity;
     CollisionShape2D coll;
    public override void _Ready()
    {
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        v=new Vector2(-1,1);
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSM = (AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSM.Start("Idle");
        velocity=new Vector2();
        p=new PlayerC(5,1.2F);

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
        gravity=(2*jumpHeight)/Mathf.Pow(timeJumpApex,2);
        jumpForce=gravity*timeJumpApex;
        velocity.y+=gravity*delta;
        if(Input.IsKeyPressed((int)KeyList.D)){

            this.Position+=Vector2.Right*movementspeed*delta;
            //velocity.x+=movementspeed;
            an.FlipH=false;
            FSM.Travel("Run");
        }
        else if(Input.IsKeyPressed((int)KeyList.A)){
            this.Position+=Vector2.Left*movementspeed*delta;
            //velocity.x-=movementspeed;
            an.FlipH=true;
            FSM.Travel("Run");
        }
        else
            FSM.Travel("Idle"); 

        if(Input.IsActionJustPressed("Jumpp")){
            if(isGrounded){
                velocity.y=-jumpForce;
                isGrounded=false;
            }
           // this.Position+=Vector2.Up*jumpspeed*delta;
        }
        velocity=MoveAndSlide(velocity,Vector2.Up);
        PlayJumping();
        Attacking();
        
  }
    public void PlayJumping(){
        if(IsOnFloor())
            isGrounded=true;
        else{
            isGrounded=false;
            if(velocity.y<0)
                FSM.Travel("Jump");
            else
                FSM.Travel("Fall");
        }
    }
    public void Attacking(){
        if(Input.IsActionJustPressed("Attack") && isGrounded){
             FSM.Travel("Attack");
        }
        else if(Input.IsActionJustPressed("Attack") && isGrounded==false){
             FSM.Travel("AirAttack");
        }
    }
    public void _on_HurtBox_body_entered(Node body){

        if(!(body is Enemy))
            return;
        GD.Print("Collision");
        Hurt();
    }
    public void Hurt(){
        CallDeferred("free");
    }
    public void _on_EnemyHit_body_entered(Node body){
        if(!(body is Enemy))
            return;
        Enemy e=(Enemy)body;
        e.Hurt();        
    }
}

