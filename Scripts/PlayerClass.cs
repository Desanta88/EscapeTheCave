public class PlayerC{
  
  public int Health{ set;get;}
  public float DamageDeal{private set;get;}
  private float CriticalDamage;
  private int critChance;

  public PlayerC(int hp,float dmg){
      Health=hp;
      DamageDeal=dmg;
      CriticalDamage=1.5F;//150% moltiplicatore che determineà quanto danno farai con dei calcoli
      critChance=1;
  }
  public bool isDead(){
      if(Health <= 0)
    
        return true;

      return false;
  }
  public void setCritDmg(){
      if(critChance == 4){
          DamageDeal= DamageDeal * (1 + CriticalDamage);//Danno di base * (1+1,5(150% moltiplicatore che determineà quanto danno farai))
          critChance=0;
      }
      else{
          DamageDeal=1.5F;
          critChance++;
      }    
  }
  
}
