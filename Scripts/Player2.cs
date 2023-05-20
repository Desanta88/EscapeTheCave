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

     public PlayerC p=new PlayerC(5,1.2F);

     private AnimatedSprite an;
     private AnimationTree tree;
     Vector2 v;
     AnimationNodeStateMachinePlayback FSM;
     Vector2 velocity;
    public override void _Ready()
    {
        an=this.GetNode<AnimatedSprite>("AnimatedSprite");
        v=new Vector2(-1,1);
        tree=this.GetNode<AnimationTree>("AnimationTree");
        FSM = (AnimationNodeStateMachinePlayback)tree.Get("parameters/playback");
        FSM.Start("Idle");
        velocity=new Vector2();

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
            an.Scale=Vector2.One;
            FSM.Travel("Run");
        }
        else if(Input.IsKeyPressed((int)KeyList.A)){
            this.Position+=Vector2.Left*movementspeed*delta;
            //velocity.x-=movementspeed;
            an.Scale=v;
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
}

