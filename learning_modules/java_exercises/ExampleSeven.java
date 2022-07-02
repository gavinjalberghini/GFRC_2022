package learning_modules.java_exercises;

public class ExampleSeven {

       public static int smallest(int a, int b, int c){
           if(a<b && a<c){
               return a;
           } 
           else{ if(b<a && b<c){
               return b;
           }
        
           else{
               return c;
           }
        }
    }
        public static int largest(int a, int b, int c){
            if(a>b && a>c){
                return a;
            } 
            else if(b>a && b>c){
                return b;
            }
            else{
                return c;
            }
    
        }
        public static int average(int a, int b, int c){
        
            return (a+b+c)/3;
        }

        public static void printVars(int a, int b, int c){
            System.out.println(a);
            System.out.println(b);
            System.out.println(c);
        }

        public static void main(String[] args){
            int smallNum;
            int largeNum;
            int avgNum;

            smallNum=smallest(7,43,21);
            largeNum=largest(7,43,21);
            avgNum=average(7,43,21);
           printVars(smallNum, largeNum, avgNum); 
    }
}
