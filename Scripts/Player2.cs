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

     private float Knockback,Knockup;
     private int hp;
     private float gravity;
     private bool isGrounded=true;

     private AnimatedSprite an;
     private AnimationTree tree;
     private AnimationNodeStateMachinePlayback FSM;

     private RayCast2D rayCastGround;
     private RayCast2D rayCastDistance;

     private RayCast2D rayCastAttack;
     private Vector2 velocity;
     private Enemy e;

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
        rayCastDistance=this.GetNode<RayCast2D>("RayCastDistance");
        rayCastAttack=this.GetNode<RayCast2D>("RayCastAttack");
        Knockback=300;
        Knockup=50;
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
            //velocity.x+=movementspeed;
            an.FlipH=false;
            FSM.Travel("Run");
            rayCastAttack.CastTo=new Vector2(36.715f,0);
           
        }
        else if(Input.IsKeyPressed((int)KeyList.A)){
            this.Position+=Vector2.Left*movementspeed*delta;
            //velocity.x-=movementspeed;
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
           // this.Position+=Vector2.Up*jumpspeed*delta;
        }
        velocity=MoveAndSlide(velocity,Vector2.Up);
        PlayJumping();
        Attacking();
        if(hp>g.p.Health){
            hp=g.p.Health;
        }
        //Stop();
        
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
        else if(Input.IsActionJustPressed("Attack") && isGrounded==false){
             FSM.Travel("AirAttack");
             AttackColliding();
        }

    }
    private void _on_HurtBox_body_entered(Node body){

        if(!(body is Enemy))
            return;
        //GD.Print("Collision");
        Hurt();
        this.Position-=new Vector2(Knockback,0);
        this.Position=new Vector2(Knockup,0);
        //velocity=MoveAndSlide(velocity,Vector2.Up);
    }
    private void Hurt(){
        g.p.Health--;
        g.LoseHeart();
        if( g.p.Health<=0)
            CallDeferred("free");
    }
   /* private void _on_EnemyHit_body_entered(Node body){
        if(!(body is Enemy))
            return;
        Enemy e=(Enemy)body;
        e.Hurt();        
    }*/
    private void _on_Area2D_body_entered(Node body){
        if(!(body is Player2))
            return;
        GD.Print("Caduto");       
    }
    private void Stop(){
        if(rayCastDistance.IsColliding()){
            e=(Enemy)rayCastDistance.GetCollider();
            e.movementspeed=0;
            //e.FSME.Travel("Idle");
           // e.FSME.Travel("Attack");
            //e.movementspeed=100;
        }
    }
    private void AttackColliding(){
        if(rayCastAttack.IsColliding()){
            Enemy e=(Enemy)rayCastAttack.GetCollider();
            g.p.setCritDmg();
            e.Hurt(g.p.DamageDeal);
            GD.Print(rayCastAttack.CastTo.x);   
            e.Position+=new Vector2((rayCastAttack.CastTo.x+100),0);
            GD.Print(e.Position);    
        }
    }
}

