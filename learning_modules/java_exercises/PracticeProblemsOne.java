package learning_modules.java_exercises;

import java.util.ArrayList;

public class PracticeProblemsOne {
    public static void main(String[] args){
    int[] nums={7,4,2,17,42};
    for(int y=0; y<=4; y++){
        System.out.println( nums[y]);
        }

        System.out.println(" ");

        int x=0;

    while(x<=4){
            System.out.println( nums[x]);
            x++;
        }

        System.out.println(" ");

    for(int element: nums){
        System.out.println(element);
            }
           
            System.out.println(" ");

        ArrayList<Integer> list=new ArrayList<Integer>();

        list.add(nums[0]);
        list.add(nums[1]);
        list.add(nums[2]);
        list.add(nums[3]);
        list.add(nums[4]);

        list.add(11);
        
        for(int element: list){
            System.out.println(element);
                }
    }
}
