import { createSlice, isPlain } from '@reduxjs/toolkit';
import axios from 'axios'
import config from './../../app/config'

export const sslFormSlice = createSlice({
    name: 'sslForm',
    initialState: {
        enteredDomain: '',
        domain: '',
        step: 0
    },
    reducers: {
        start: (state, action) => {
            state.domain = state.enteredDomain;
            state.step = 1;
        },
        changeEnteredDomain: (state, action) => {
            state.enteredDomain = action.payload;
        }
    }
});

export const { start, changeEnteredDomain } = sslFormSlice.actions;

export const startAsync = state => async (dispatch, getState) => {

    let result = await axios.post(`${config.site}api/ssl/start`, {
        domains: [ getState().sslForm.enteredDomain ]
    });

    console.log(result);

};

export const selectState = state => state.sslForm.step;

export const getDomainForUse = state => state.sslForm.domain;

export const getEnteredDomain = state => state.sslForm.getEnteredDomain;

export default sslFormSlice.reducer;
