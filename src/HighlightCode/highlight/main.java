public class MainAct extends AppCompatActivity {

         static List<OnReceiveListener> onReceiveList = new ArrayList<OnReceiveListener>();

        public void attachOnReceiveListener(OnReceiveListener listener) {
            if (!onReceiveList.contains(listener)) {
                onReceiveList.add(listener);
            }
        }

        public interface OnReceiveListener {
            // This can be any number of events to be sent to the activity
            void receivePreview(Object... obj);
        }

        public onReceivePreview(Object... obj) {
                 for (OnReceiveListener listener : onReceiveList) {
                    listener.receivePreview(obj);
                }
        }
     }

    public class OneFragment extends Fragment implements OnReceiveListener{


        @Override
        public void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            MainAct  mainAct = new MainAct();
            mainAct.attachOnReceiveListener(this);

        }

        @Override
        public void receivePreview(Object... obj) {

        }
    }

    public class TwoFragment extends Fragment implements OnReceiveListener{

        @Override
        public void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);
            MainAct  mainAct = new MainAct();
            mainAct.attachOnReceiveListener(this);

        }
        @Override
        public void receivePreview(Object... obj) {

        }
    } 
