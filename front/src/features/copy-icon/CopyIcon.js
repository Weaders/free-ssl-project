import { toast } from 'react-toastify';
import { getTranslate } from './../../app/store-data/main';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import { useSelector } from 'react-redux'
import { makeStyles } from '@material-ui/core/styles';
import FileCopyIcon from '@material-ui/icons/FileCopy';

const useStyles = makeStyles((theme) => ({
    icon: {
        width: '20px'
    },
    link: {
        verticalAlign: 'text-top',
        marginLeft: '8px',
    }
}));

export default function CopyIcon(props){

    const translate = useSelector(getTranslate);
    const styles = useStyles();

    return (
        <CopyToClipboard onCopy={() => toast.info(translate("copied"))} text={props.text}>
            <a href="#" className={styles.link} title={translate("copy")} onClick={() => false}><FileCopyIcon className={styles.icon}></FileCopyIcon></a>
        </CopyToClipboard>
    );
    
}