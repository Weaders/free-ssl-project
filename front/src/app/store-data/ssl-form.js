
import { createSlice } from '@reduxjs/toolkit';
import axios from 'axios'
import config from './../../app/config'
import _ from 'lodash'
import Request from './../request'
import { setIsLoading } from './main'

export const sslFormSlice = createSlice({
    name: 'sslForm',
    initialState: {
        domains: [],
        challenges: [],
        sessionId: '',
        certData: null
    },
    reducers: {
        addDomain: (state, action) => {
            state.domains.push(action.payload);
        },
        addChallenge: (state, action) => {
            state.challenges.push(action.payload);
        },
        clearChallenges: (state, action) => {
            state.challenges = [];
        },
        setSessionId: (state, action) => {
            state.sessionId = action.payload;
        }, 
        removeDomain: (state, action) => {
            state.domains = _.remove(state.domains, (d) => d !== action.payload);
        },
        setCertData: (state, action) => {
            state.certData = {
                privateKey: action.payload.privateKey,
                pemKey: action.payload.pemKey
            };
        }
    }
});

export const { start, removeDomain, addDomain, clearChallenges, setCertData } = sslFormSlice.actions;

export const startAsync = state => async (dispatch, getState) => {

    try {

        dispatch(setIsLoading(true));

        let result = await axios.post(`${config.site}ssl/start`, {
            domains: getState().sslForm.domains
        });

        result.data.chalengeResults.forEach(ele => dispatch(sslFormSlice.actions.addChallenge(ele)));
        dispatch(sslFormSlice.actions.setSessionId(result.data.id));
    }
    finally {
        dispatch(setIsLoading(false));
    }

};

export const getCertAsync = state => async (dispatch, getState) => {

    let state = getState();

    try {

        dispatch(setIsLoading(true));

        let result = await axios.post(`${config.site}ssl/download`, {
            id: state.sslForm.sessionId
        });

        dispatch(sslFormSlice.actions.setCertData(result.data));

    }
    finally {
        dispatch(setIsLoading(false));
    }

};

export const selectState = state => state.sslForm.step;

export const getDomains = state => state.sslForm.domains;

export const getEnteredDomain = state => state.sslForm.getEnteredDomain;

export const getChallenges = state => state.sslForm.challenges;

export const getCertData = state => state.sslForm.certData;

export default sslFormSlice.reducer;
