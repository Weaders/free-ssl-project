import { createSlice, createSelector } from '@reduxjs/toolkit';

import ruLangData from './../langs/ru.json'

export const LANG_RU = 'ru';
export const LANG_EN = 'eng';

export const mainDataSlice = createSlice({
    name: 'main',
    initialState: {
        lang: LANG_RU
    },
    reducers: {
    }
});

const getLangData = state => {

    switch(state.main.lang){
        case LANG_RU:
            return ruLangData;
    }

    return null;

};

export const getTranslate = createSelector(
    getLangData, 
    (langData) => (placehHolder) =>{
        
        if (langData && langData[placehHolder])
            return langData[placehHolder]

        return placehHolder;;

    }
);

export default mainDataSlice.reducer;