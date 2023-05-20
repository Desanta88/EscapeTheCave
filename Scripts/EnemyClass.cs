public class EnemyC{

  public float Health{set;get;}
  public int DamageDeal{private set;get;}

  public EnemyC(float hp){
      Health=hp;
      DamageDeal=1;
  }

  public bool isDead(){
      if(Health <= 0)
        return true;

      return false;
  }
  
}