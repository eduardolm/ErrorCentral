import React from 'react';
import {useHistory} from 'react-router-dom';
import clsx from 'clsx';
import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';
import Drawer from '@material-ui/core/Drawer';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import ListOutlinedIcon from '@material-ui/icons/ListOutlined';
import PersonAddOutlinedIcon from '@material-ui/icons/PersonAddOutlined';
import UpdateOutlinedIcon from '@material-ui/icons/UpdateOutlined';
import DeleteOutlineOutlinedIcon from '@material-ui/icons/DeleteOutlineOutlined';
import IconButton from '@material-ui/core/IconButton';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Collapse from '@material-ui/core/Collapse';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
    list: {
        width: 250,
    },
    fullList: {
        width: 'auto',
    },
    root: {
        '& > *': {
            margin: theme.spacing(1)
        }
    }
}));

type Anchor = 'top' | 'left' | 'bottom' | 'right';

export default function TemporaryDrawer() {
    const history = useHistory();
    const classes = useStyles();
    const [state, setState] = React.useState({
        top: false,
        left: false,
        bottom: false,
        right: false,
    });

    const toggleDrawer = (anchor: Anchor, open: boolean) => (
        event: React.KeyboardEvent | React.MouseEvent,
    ) => {
        if (
            event.type === 'keydown' &&
            ((event as React.KeyboardEvent).key === 'Tab' ||
                (event as React.KeyboardEvent).key === 'Shift')
        ) {
            return;
        }

        setState({ ...state, [anchor]: open });
    };

    const list = (anchor: Anchor) => (
        <div
            className={clsx(classes.list, {
                [classes.fullList]: anchor === 'top' || anchor === 'bottom',
            })}
            role="presentation"
            onClick={toggleDrawer(anchor, false)}
            onKeyDown={toggleDrawer(anchor, false)}
        >
            <List component="nav">
                    <ListItem>
                        <Button onClick={() => {history.push('/user/list')}}>
                            <ListItemIcon> <ListOutlinedIcon fontSize="large"/> </ListItemIcon>
                            <ListItemText primary="Listar" />
                        </Button>
                    </ListItem>
                    <ListItem>
                        <Button onClick={() => {history.push('/user/create')}}>
                            <ListItemIcon> <PersonAddOutlinedIcon fontSize="large"/> </ListItemIcon>
                            <ListItemText primary="Cadstrar" />
                        </Button>
                    </ListItem>
                    <ListItem>
                        <Button onClick={() => {history.push('/user/update')}}>
                            <ListItemIcon> <UpdateOutlinedIcon fontSize="large"/> </ListItemIcon>
                            <ListItemText primary="Alterar" />
                        </Button>
                    </ListItem>
                    <ListItem>
                        <Button onClick={() => {history.push('/user/delete')}}>
                            <ListItemIcon> <DeleteOutlineOutlinedIcon fontSize="large"/> </ListItemIcon>
                            <ListItemText primary="Excluir" />
                        </Button>
                    </ListItem>
            </List>
        </div>
    );

    return (
        <div className={classes.root}>

            {(['left', 'right', 'top', 'bottom'] as Anchor[]).map((anchor) => (
                <React.Fragment key={anchor}>
                    <Button variant="contained" color="primary" onClick={toggleDrawer(anchor, true)}>{anchor}</Button>
                    <Drawer anchor={anchor} open={state[anchor]} onClose={toggleDrawer(anchor, false)}>
                        {list(anchor)}
                    </Drawer>
                </React.Fragment>
            ))}
        </div>
    );
}
