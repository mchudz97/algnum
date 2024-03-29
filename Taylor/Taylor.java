import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import static java.lang.Math.*;
public class Taylor {


    private double[] arr1;
    private double[] arr2;

    Taylor(){

    }
    public void writeToFile(double start, double end, int N, double sensitivity, int depth, String name ) throws IOException {
        double[][] arr= this.compression(this.generateAllErrors(start,end,N, sensitivity), depth);
        String str="";
        double step=(double)depth*sensitivity;
        BufferedWriter writer = new BufferedWriter(new FileWriter(name));
        for(int i=0;i<arr[0].length;i++) {
            str = (start+(double)i*step)+"\t" + arr[0][i] + "\t" + arr[1][i] + "\t" + arr[2][i] + "\t" + arr[3][i] + "\n";
            str.replace(".",",");
            writer.write(str);
        }
        writer.close();
    }
    public double[][] generateAllErrors(double start,double end, int N, double sensitivity){
        double[][] finalList= new double[4][(int)((end-start)*10/sensitivity)];
        int i=0;
        double l=start;
        //for(double l=start;l<end; l+=sensitivity){
        for(int j=0;j<(int)((end-start)*10/sensitivity); j++){
            System.out.println(j);
            this.fillArrays(l, N);
            double[] temp=this.generateErrors(l);
            finalList[0][j]=temp[0];
            finalList[1][j]=temp[1];
            finalList[2][j]=temp[2];
            finalList[3][j]=temp[3];
            l+=sensitivity;

        }
        return finalList;
    }
    public double[][] compression(double[][] list, int depth){
        double[][] tmp= new double[4][list[0].length/depth];
        for(int i=0;i<list[0].length/depth;i++){
            double sum=0, sum2=0, sum3=0, sum4=0;
            for(int j=i*depth;j<depth*(i+1);j++){
                sum+=list[0][j];
                sum2+=list[1][j];
                sum3+=list[2][j];
                sum4+=list[3][j];
            }
            tmp[0][i]=sum/(double)depth;
            tmp[1][i]=sum2/(double)depth;
            tmp[2][i]=sum3/(double)depth;
            tmp[3][i]=sum4/(double)depth;
        }
        return tmp;
    }

    public double[] generateErrors(double l){
        double[] arr= new double[4];

            double ln= log(l);
            arr[0]=this.getResult(arr1)-ln;
            arr[1]=this.getResult(arr2)-ln;
            arr[2]=this.getResult(this.reverse(arr1))-ln;
            arr[3]=this.getResult(this.reverse(arr2))-ln;

        return arr;
    }

    public void fillArrays(double l, int N){

        this.setArr1(this.taylorSp1(l, N));

        this.setArr2(this.taylorSp2(l, N));

    }

    public double getResult(double[] arr){
        double sum=0;
        for(int i=0;i<arr.length;i++){
            sum=sum+arr[i];

        }
        return sum;
    }
    public double[] reverse(double[] arr){
        double[] tmparr= new double[arr.length];
        for(int i=arr.length-1;i>=0;i--){
            tmparr[arr.length-1-i]=arr[i];
        }
        return tmparr;
    }

    public double calc1(double x, int n){ //wyliczanie pojedynczego wyrazu z wzoru sumy
        return pow(-1, n+1)*pow(x-1,n)/n;
    }


    private double[]  taylorSp1(double l, int N){ //ze wzoru taylora
        double[] arr= new double[N];
        for(int n=1;n<=N;n++){
            arr[n-1]=calc1(l,n);


        }

        return arr;
    }

    private double[] taylorSp2(double l, int N){ //wyznaczajac wzor na kolejny element ciagu
        double sum=l-1;
        double sum2=0;
        double[] arr=new double[N];
        for (int n=1; n<=N;n++){

            arr[n-1]=sum;

            sum*=((double)(-n)*(l-1)/(double)(n+1));

        }

        return arr;

    }


    private double pow(double l, int p){ //funkcja potegujaca
        if(l==1) return 1;
        if(l==-1){
            if(p%2==0) return 1;
            else return -1;
        }

        if(l==0){
            return 0;
        }
        if(p==0){
            return 1;
        }

        return l* pow(l, p-1);
    }

    public double[] getArr2() {
        return arr2;
    }



    public void setArr2(double[] arr2) {
        this.arr2 = arr2;
    }

    public double[] getArr1() {
        return arr1;
    }

    public void setArr1(double[] arr1) {
        this.arr1 = arr1;
    }



    public static void main(String[] args) throws IOException {
        Taylor t1=new Taylor();
        //t1.compression(t1.generateAllErrors(0.7,1.5,100, 0.0001), 100);
        long startTime = System.nanoTime();
        t1.writeToFile(0.5, 1.5, 1000, 0.00001, 1000, "taylor1.csv" );
        System.out.println(((System.nanoTime() - startTime)/1000000 + "ms"));
        /* startTime = System.nanoTime();
        t1.writeToFile(0.5, 1.5, 1000, 0.01, 1, "taylor2.csv" );
        System.out.println(((System.nanoTime() - startTime)/1000000 + "ms"));*/

       /* System.out.println( t1.getReversedResult(t1.getArr1()));
        System.out.println( t1.getReversedResult(t1.getArr2()));
        System.out.println(t1.getResult(t1.getArr1()));
        System.out.println(t1.getResult(t1.getArr2()));*/
    }

}
