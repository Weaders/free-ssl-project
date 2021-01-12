
import { createSlice } from '@reduxjs/toolkit';
import axios from 'axios'
import config from './../../app/config'
import _ from 'lodash'

export const sslFormSlice = createSlice({
    name: 'sslForm',
    initialState: {
        domains: [],
        challenges: [],
        sessionId: ''
    },
    reducers: {
        addDomain: (state, action) => {
            state.domains.push(action.payload);
        },
        addChallenge: (state, action) => {
            state.challenges.push(action.payload);
        },
        setSessionId: (state, action) => {
            state.sessionId = action.payload;
        }, 
        removeDomain: (state, action) => {
            state.domains = _.remove(state.domains, (d) => d !== action.payload);
        }
    }
});

export const { start, removeDomain, addDomain } = sslFormSlice.actions;

export const startAsync = state => async (dispatch, getState) => {

    let result = await axios.post(`${config.site}api/ssl/start`, {
        domains: getState().sslForm.domains
    });

    result.data.chalengeResults.forEach(ele => dispatch(sslFormSlice.actions.addChallenge(ele)));
    dispatch(sslFormSlice.actions.setSessionId(result.data.id));

};

export const getCertAsync = state => async (dispatch, getState) => {

    let state = getState();

    let result = await axios.post(`${config.site}api/ssl/download`, {
        id: state.sslForm.sessionId
    });

    console.log(result);

};

export const selectState = state => state.sslForm.step;

export const getDomains = state => state.sslForm.domains;

export const getEnteredDomain = state => state.sslForm.getEnteredDomain;

export const getChallenges = state => state.sslForm.challenges;

export default sslFormSlice.reducer;
