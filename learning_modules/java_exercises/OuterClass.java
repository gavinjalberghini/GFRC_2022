package learning_modules.java_exercises;

public class OuterClass {

    private int x;
    private int y;
    private int z;

    private static int mutliplier=1;

    public class shooter{
        public shooter( int X, int Y, int Z){
            x=X;
            y=Y;
            z=Z;
        }
        
        public void shoot(){
            System.out.println("x: " + (x*mutliplier));
            System.out.println("y: " + (y*mutliplier));
            System.out.println("z: " + (z*mutliplier));
        }
    }

    public static class force{
        public static void changeForce(int mult){
            mutliplier=mult;
        }
    }
}
