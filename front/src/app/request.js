import axios from 'axios'

export default class Request 
{
    constructor(){}

    static async post(reqObj){

        const data = await axios.post(reqObj);
        console.log(data);
        return data;

    }

}