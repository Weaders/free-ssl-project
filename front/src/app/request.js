import axios from 'axios'
import { toast } from 'react-toastify';
import { getTranslate, setIsLoading, getLangData } from './store-data/main';

export default class Request 
{
    static async post(url, reqObj, dispatch, getState){

        try {

            dispatch(setIsLoading(true));
    
            return await axios.post(url, reqObj); 

        }
        catch(e) {

            const translate = getTranslate(getState());

            if (e.isAxiosError && e.response && e.response.data){
                toast.warn(translate(e.response.data.msg))
            } else {
                toast.error(translate('server_error'))
            }

            return null;
        }
        finally {
            dispatch(setIsLoading(false));
        }
        
    }

}